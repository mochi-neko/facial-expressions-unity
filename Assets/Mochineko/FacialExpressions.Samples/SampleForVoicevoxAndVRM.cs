#nullable enable
using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.FacialExpressions.Blink;
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
using NUnit.Framework;
using UnityEngine;
using UniVRM10;
using VRMShaders;
using Object = UnityEngine.Object;

namespace Mochineko.FacialExpressions.Samples
{
    internal sealed class SampleForVoicevoxAndVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        [SerializeField] private string text = string.Empty;
        [SerializeField] private int speakerID;
        [SerializeField] private AudioSource? audioSource = null;

        private SequentialLipAnimator? lipAnimator;
        private SequentialEyelidAnimator? eyelidAnimator;
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
            var binary = await File.ReadAllBytesAsync(
                path,
                this.GetCancellationTokenOnDestroy());

            var instance = await LoadVRMAsync(
                binary,
                this.GetCancellationTokenOnDestroy());

            var lipMorpher = new VRMLipMorpher(instance.Runtime.Expression);
            lipAnimator = new SequentialLipAnimator(lipMorpher);

            var eyelidMorper = new VRMEyelidMorpher(instance.Runtime.Expression);
            eyelidAnimator = new SequentialEyelidAnimator(eyelidMorper);

            var eyelidFrames = RandomBlinkAnimationGenerator.Generate(
                Eyelid.Both,
                blinkCount: 20);

            eyelidAnimator.AnimateAsync(
                    eyelidFrames,
                    loop: true,
                    this.GetCancellationTokenOnDestroy())
                .Forget();
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

            var lipFrames = AudioQueryConverter.Convert(audioQuery);

            await UniTask.SwitchToMainThread(cancellationToken);

            // Play AudioClip.
            audioSource.clip = audioClip;
            audioSource.Play();

            lipAnimator
                ?.AnimateAsync(lipFrames, cancellationToken)
                .Forget();
        }
    }
}