#nullable enable
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.FacialExpressions.Blink;
using Mochineko.FacialExpressions.Extensions.VRM;
using UnityEngine;
using UniVRM10;
using VRMShaders;

namespace Mochineko.FacialExpressions.Samples
{
    internal sealed class BlinkerSampleForVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        private SequentialSequentialEyelidAnimator? eyelidAnimator;

        private async void Start()
        {
            var binary = await File.ReadAllBytesAsync(
                path,
                this.GetCancellationTokenOnDestroy());

            var instance = await LoadVRMAsync(
                binary,
                this.GetCancellationTokenOnDestroy());

            var eyelidMorper = new VRMEyelidMorpher(instance.Runtime.Expression);
            eyelidAnimator = new SequentialSequentialEyelidAnimator(eyelidMorper);

            var frames = RandomBlinkAnimationGenerator.Generate(
                Eyelid.Both, 
                blinkCount: 20);
            
            eyelidAnimator.AnimateAsync(
                    frames,
                    loop:true,
                    this.GetCancellationTokenOnDestroy())
                .Forget();
        }
        
        private static async UniTask<Vrm10Instance> LoadVRMAsync(
            byte[] binaryData,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Vrm10.LoadBytesAsync(
                bytes:binaryData,
                canLoadVrm0X:true,
                controlRigGenerationOption:ControlRigGenerationOption.None,
                showMeshes:true,
                awaitCaller:new RuntimeOnlyAwaitCaller(),
                materialGenerator:null,
                vrmMetaInformationCallback:null,
                ct:cancellationToken
            );
        }
    }
}