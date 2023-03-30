#nullable enable
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.FacialExpressions.Extensions.VOICEVOX;
using Mochineko.FacialExpressions.Extensions.VRM;
using Mochineko.FacialExpressions.VisemeLipSync;
using Mochineko.VOICEVOX_API.QueryCreation;
using Newtonsoft.Json;
using UnityEngine;
using UniVRM10;
using VRMShaders;

namespace Mochineko.FacialExpressions.Samples
{
    internal sealed class VisemeLipSyncSampleForVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        [SerializeField] private string audioQueryJson = string.Empty;
        
        private SequentialLipAnimator? lipAnimator;

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

        [ContextMenu(nameof(Play))]
        public void Play()
        {
            var audioQuery = JsonConvert.DeserializeObject<AudioQuery>(audioQueryJson);
            if (audioQuery == null)
            {
                Debug.LogError("Failed to deserialize AudioQuery.");
                return;
            }
            
            var frames = AudioQueryConverter.Convert(audioQuery);

            lipAnimator?
                .AnimateAsync(frames, this.GetCancellationTokenOnDestroy())
                .Forget();
        }
    }
}
