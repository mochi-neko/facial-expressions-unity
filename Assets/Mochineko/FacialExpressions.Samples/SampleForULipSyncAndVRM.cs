#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.FacialExpressions.Blink;
using Mochineko.FacialExpressions.Emotion;
using Mochineko.FacialExpressions.Extensions.uLipSync;
using Mochineko.FacialExpressions.Extensions.VRM;
using Mochineko.FacialExpressions.LipSync;
using Mochineko.VOICEVOX_API;
using UnityEngine;
using UniVRM10;
using VRMShaders;

namespace Mochineko.FacialExpressions.Samples
{
    internal sealed class SampleForULipSyncAndVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        [SerializeField] private BasicEmotion basicEmotion = BasicEmotion.Neutral;
        [SerializeField] private float emotionWeight = 1f;
        [SerializeField] private float emotionFollowingTime = 1f;
        [SerializeField] private uLipSync.uLipSync? uLipSync = null;

        private ULipSyncAnimator? lipAnimator;
        private IDisposable? eyelidAnimationLoop;
        private ExclusiveFollowingEmotionAnimator<BasicEmotion>? emotionAnimator;

        private async void Start()
        {
            if (uLipSync == null)
            {
                throw new NullReferenceException(nameof(uLipSync));
            }

            VoiceVoxBaseURL.BaseURL = "http://127.0.0.1:50021";

            var binary = await File.ReadAllBytesAsync(
                path,
                this.GetCancellationTokenOnDestroy());

            var instance = await LoadVRMAsync(
                binary,
                this.GetCancellationTokenOnDestroy());

            var lipMorpher = new VRMLipMorpher(instance.Runtime.Expression);
            var followingLipAnimator = new FollowingLipAnimator(lipMorpher, followingTime: 0.15f);
            lipAnimator = new ULipSyncAnimator(
                followingLipAnimator,
                uLipSync,
                phonomeMap: new Dictionary<string, Viseme>
                {
                    ["A"] = Viseme.aa,
                    ["I"] = Viseme.ih,
                    ["U"] = Viseme.ou,
                    ["E"] = Viseme.E,
                    ["O"] = Viseme.oh,
                    ["N"] = Viseme.sil,
                });

            var eyelidFrames = ProbabilisticEyelidAnimationGenerator.Generate(
                Eyelid.Both,
                blinkCount: 20);

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
            lipAnimator?.Dispose();
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

        [ContextMenu(nameof(Emote))]
        public void Emote()
        {
            emotionAnimator?
                .Emote(new EmotionSample<BasicEmotion>(
                    basicEmotion,
                    weight: emotionWeight));
        }
    }
}
