#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.FacialExpressions.Blink;
using Mochineko.FacialExpressions.Emotion;
using Mochineko.FacialExpressions.Extensions.VOICEVOX;
using Mochineko.FacialExpressions.Extensions.VRM;
using Mochineko.FacialExpressions.LipSync;
using Mochineko.Relent.Resilience;
using Mochineko.Relent.UncertainResult;
using Mochineko.SimpleAudioCodec;
using Mochineko.VOICEVOX_API;
using Mochineko.VOICEVOX_API.QueryCreation;
using Mochineko.VOICEVOX_API.Synthesis;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;
using UniVRM10;
using VRMShaders;

namespace Mochineko.FacialExpressions.Samples
{
    // ReSharper disable once InconsistentNaming
    internal sealed class SampleForVoiceVoxAndVRM : MonoBehaviour
    {
        [SerializeField]
        private string path = string.Empty;

        [SerializeField]
        private string text = string.Empty;

        [SerializeField]
        private int speakerID;

        [SerializeField]
        private AudioSource? audioSource = null;

        [SerializeField]
        private bool skipSpeechSynthesis = false;

        [SerializeField]
        private BasicEmotion basicEmotion = BasicEmotion.Neutral;

        [SerializeField]
        private float emotionWeight = 1f;

        [SerializeField]
        private float emotionFollowingTime = 1f;

        private FollowingLipAnimator? lipAnimator;
        private IDisposable? eyelidAnimationLoop;
        private ExclusiveFollowingEmotionAnimator<BasicEmotion>? emotionAnimator;
        private AudioClip? audioClip;

        private readonly IPolicy<AudioQuery> queryCreationPolicy
            = PolicyFactory.BuildQueryCreationPolicy();

        private readonly IPolicy<Stream> synthesisPolicy
            = PolicyFactory.BuildSynthesisPolicy();

        private void Awake()
        {
            Assert.IsNotNull(audioSource);
        }

        private async void Start()
        {
            VoiceVoxBaseURL.BaseURL = "http://127.0.0.1:50021";

            var binary = await File.ReadAllBytesAsync(
                path,
                this.GetCancellationTokenOnDestroy());

            var instance = await LoadVRMAsync(
                binary,
                this.GetCancellationTokenOnDestroy());

            var lipMorpher = new VRMLipMorpher(instance.Runtime.Expression);
            lipAnimator = new FollowingLipAnimator(lipMorpher);

            var eyelidFrames = ProbabilisticEyelidAnimationGenerator
                .Generate(
                    Eyelid.Both,
                    blinkCount: 20
                );

            var eyelidMorpher = new VRMEyelidMorpher(instance.Runtime.Expression);
            var eyelidAnimator = new SequentialEyelidAnimator(eyelidMorpher);
            eyelidAnimationLoop = new LoopEyelidAnimator(eyelidAnimator, eyelidFrames);

            var emotionMorpher = new VRMEmotionMorpher<BasicEmotion>(
                instance.Runtime.Expression,
                keyMap: new Dictionary<BasicEmotion, ExpressionKey>
                {
                    [BasicEmotion.Neutral] = ExpressionKey.Neutral,
                    [BasicEmotion.Happy] = ExpressionKey.Happy,
                    [BasicEmotion.Sad] = ExpressionKey.Sad,
                    [BasicEmotion.Angry] = ExpressionKey.Angry,
                    [BasicEmotion.Fearful] = ExpressionKey.Neutral,
                    [BasicEmotion.Surprised] = ExpressionKey.Surprised,
                    [BasicEmotion.Disgusted] = ExpressionKey.Neutral,
                });
            emotionAnimator = new ExclusiveFollowingEmotionAnimator<BasicEmotion>(
                emotionMorpher,
                followingTime: emotionFollowingTime);
        }

        private void Update()
        {
            lipAnimator?.Update();
            emotionAnimator?.Update();
        }

        private void OnDestroy()
        {
            if (audioClip != null)
            {
                Destroy(audioClip);
                audioClip = null;
            }
            eyelidAnimationLoop?.Dispose();
        }

        private static async UniTask<Vrm10Instance> LoadVRMAsync(
            byte[] binaryData,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Vrm10.LoadBytesAsync(
                bytes: binaryData,
                canLoadVrm0X: true,
                controlRigGenerationOption: ControlRigGenerationOption.None,
                showMeshes: true,
                awaitCaller: new RuntimeOnlyAwaitCaller(),
                materialGenerator: null,
                vrmMetaInformationCallback: null,
                ct: cancellationToken
            );
        }

        [ContextMenu(nameof(SynthesisSpeech))]
        public void SynthesisSpeech()
        {
            SynthesisAsync(
                    text,
                    speakerID,
                    this.GetCancellationTokenOnDestroy())
                .Forget();
        }

        private async UniTask SynthesisAsync(
            string text,
            int speakerID,
            CancellationToken cancellationToken)
        {
            if (audioSource == null)
            {
                Debug.LogError(audioSource);
                return;
            }

            if (lipAnimator == null)
            {
                return;
            }

            if (audioClip != null)
            {
                Destroy(audioClip);
                audioClip = null;
            }

            audioSource.Stop();

            await UniTask.SwitchToThreadPool();

            var audioQuery = (await queryCreationPolicy.ExecuteAsync(
                    async innerCancellationToken => await QueryCreationAPI.CreateQueryAsync(
                        HttpClientPool.PooledClient,
                        text: text,
                        speaker: speakerID,
                        coreVersion: null,
                        cancellationToken: innerCancellationToken),
                    cancellationToken)
                ).Unwrap();

            // Synthesize speech from AudioQuery by VOICEVOX synthesis API.
            var stream = (await synthesisPolicy.ExecuteAsync(
                    async innerCancellationToken => await SynthesisAPI.SynthesizeAsync(
                        HttpClientPool.PooledClient,
                        audioQuery: audioQuery,
                        speaker: speakerID,
                        enableInterrogativeUpspeak: null,
                        coreVersion: null,
                        cancellationToken: innerCancellationToken),
                    cancellationToken)
                ).Unwrap();

            if (!skipSpeechSynthesis)
            {
                try
                {
                    // Decode WAV data to AudioClip by SimpleAudioCodec WAV decoder.
                    audioClip = await WaveDecoder.DecodeBlockByBlockAsync(
                        stream: stream,
                        fileName: "Synthesis.wav",
                        cancellationToken: cancellationToken);

                    Debug.Log($"[VOICEVOX_API.Samples] Succeeded to decode audio, " +
                              $"samples:{audioClip.samples}, " +
                              $"frequency:{audioClip.frequency}, " +
                              $"channels:{audioClip.channels}, " +
                              $"length:{audioClip.length}.");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    return;
                }
                finally
                {
                    await stream.DisposeAsync();
                }

                await UniTask.SwitchToMainThread(cancellationToken);

                // Play AudioClip.
                audioSource.clip = audioClip;
                audioSource.PlayDelayed(0.1f);
            }

            var lipFrames = AudioQueryConverter
                .ConvertToSequentialAnimationFrames(audioQuery);

            await UniTask.Delay(
                TimeSpan.FromSeconds(0.1f),
                cancellationToken: cancellationToken);

            lipAnimator
                .SetTargetAsync(lipFrames, cancellationToken)
                .Forget();
        }

        [ContextMenu(nameof(Emote))]
        public void Emote()
        {
            emotionAnimator?
                .Emote(new EmotionSample<BasicEmotion>(
                    basicEmotion,
                    weight: emotionWeight));
        }
    }

    // ReSharper disable once InconsistentNaming
}
