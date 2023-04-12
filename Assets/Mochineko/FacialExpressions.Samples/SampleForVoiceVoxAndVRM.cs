#nullable enable
using System;
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
using Object = UnityEngine.Object;

namespace Mochineko.FacialExpressions.Samples
{
    // ReSharper disable once InconsistentNaming
    internal sealed class SampleForVoiceVoxAndVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        [SerializeField] private string text = string.Empty;
        [SerializeField] private int speakerID;
        [SerializeField] private AudioSource? audioSource = null;
        [SerializeField] private bool skipSpeechSynthesis = false;
        [SerializeField] private Emotion.Emotion emotion = Emotion.Emotion.Neutral;
        [SerializeField] private float emotionWeight = 1f;
        [SerializeField] private float emotionFollowingTime = 1f;
        
        private ILipAnimator? lipAnimator;
        private IEyelidAnimator? eyelidAnimator;
        private ExclusiveFollowingEmotionAnimator<Emotion.Emotion>? emotionAnimator;
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

            var eyelidMorpher = new VRMEyelidMorpher(instance.Runtime.Expression);
            eyelidAnimator = new SequentialEyelidAnimator(eyelidMorpher);

            var eyelidFrames = ProbabilisticEyelidAnimationGenerator.Generate(
                Eyelid.Both,
                blinkCount: 20);

            eyelidAnimator.AnimateAsync(
                    eyelidFrames,
                    loop: true,
                    this.GetCancellationTokenOnDestroy())
                .Forget();
            
            var emotionMorpher = new VRMEmotionMorpher(instance.Runtime.Expression);
            emotionAnimator = new ExclusiveFollowingEmotionAnimator<Emotion.Emotion>(
                emotionMorpher,
                followingTime: emotionFollowingTime);
        }

        private void Update()
        {
            emotionAnimator?.Update();
        }

        private void OnDestroy()
        {
            if (audioClip != null)
            {
                Object.Destroy(audioClip);
                audioClip = null;
            }
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

            if (audioClip != null)
            {
                Object.Destroy(audioClip);
                audioClip = null;
            }

            audioSource.Stop();

            await UniTask.SwitchToThreadPool();

            AudioQuery audioQuery;
            var createQueryResult = await queryCreationPolicy.ExecuteAsync(
                async innerCancellationToken => await QueryCreationAPI.CreateQueryAsync(
                    HttpClientPool.PooledClient,
                    text: text,
                    speaker: speakerID,
                    coreVersion: null,
                    cancellationToken: innerCancellationToken),
                cancellationToken);
            if (createQueryResult is IUncertainSuccessResult<AudioQuery> createQuerySuccess)
            {
                audioQuery = createQuerySuccess.Result;
                Debug.Log(
                    $"[VOICEVOX_API.Samples] Succeeded to create query from text:{text} -> {JsonConvert.SerializeObject(audioQuery)}.");
            }
            else if (createQueryResult is IUncertainRetryableResult<AudioQuery> createQueryRetryable)
            {
                Debug.LogError(
                    $"[VOICEVOX_API.Samples] Failed to create query because -> {createQueryRetryable.Message}.");
                return;
            }
            else if (createQueryResult is IUncertainFailureResult<AudioQuery> createQueryFailure)
            {
                Debug.LogError(
                    $"[VOICEVOX_API.Samples] Failed to create query because -> {createQueryFailure.Message}.");
                return;
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(createQueryResult));
            }

            // Synthesize speech from AudioQuery by VOICEVOX synthesis API.
            Stream stream;
            var synthesisResult = await synthesisPolicy.ExecuteAsync(
                async innerCancellationToken => await SynthesisAPI.SynthesizeAsync(
                    HttpClientPool.PooledClient,
                    audioQuery: audioQuery,
                    speaker: speakerID,
                    enableInterrogativeUpspeak: null,
                    coreVersion: null,
                    cancellationToken: innerCancellationToken),
                cancellationToken);
            if (synthesisResult is IUncertainSuccessResult<Stream> synthesisSuccess)
            {
                stream = synthesisSuccess.Result;
                Debug.Log($"[VOICEVOX_API.Samples] Succeeded to synthesis speech from text:{text}.");
            }
            else if (synthesisResult is IUncertainRetryableResult<Stream> synthesisRetryable)
            {
                Debug.LogError(
                    $"[VOICEVOX_API.Samples] Failed to synthesis speech because -> {synthesisRetryable.Message}.");
                return;
            }
            else if (synthesisResult is IUncertainFailureResult<Stream> synthesisFailure)
            {
                Debug.LogError(
                    $"[VOICEVOX_API.Samples] Failed to synthesis speech because -> {synthesisFailure.Message}.");
                return;
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(synthesisResult));
            }

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
            
            var lipFrames = AudioQueryConverter.ConvertToSequentialAnimationFrames(audioQuery);

            await UniTask.Delay(
                TimeSpan.FromSeconds(0.1f),
                cancellationToken:cancellationToken);
            
            lipAnimator
                ?.AnimateAsync(lipFrames, cancellationToken)
                .Forget();
        }
        
        [ContextMenu(nameof(Emote))]
        public void Emote()
        {
            emotionAnimator?
                .Emote(new EmotionSample<Emotion.Emotion>(
                    emotion,
                    weight: emotionWeight));
        }
    }
}