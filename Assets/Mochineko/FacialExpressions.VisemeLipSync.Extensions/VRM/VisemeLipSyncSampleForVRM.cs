#nullable enable
using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniVRM10;
using VRMShaders;

namespace Mochineko.FacialExpressions.VisemeLipSync.Extensions.VRM
{
    internal sealed class VisemeLipSyncSampleForVRM : MonoBehaviour
    {
        [SerializeField] private string path = string.Empty;
        [SerializeField] private Viseme viseme = Viseme.aa;
        [SerializeField, Range(0f, 1f)] private float weight = 0f;
        
        private ILipMorpher? lipMorpher;

        [ContextMenu(nameof(Morph))]
        public void Morph()
        {
            if (lipMorpher is null)
            {
                return;
            }
            
            lipMorpher.MorphInto(new LipSample(viseme, weight));
        }
        
        private async void Start()
        {
            var binary = await File.ReadAllBytesAsync(
                path,
                this.GetCancellationTokenOnDestroy());
            
            var instance = await LoadVRMAsync(
                binary,
                this.GetCancellationTokenOnDestroy());

            lipMorpher = new VRMLipMorpher(instance.Runtime.Expression);
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
