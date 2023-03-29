#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    public sealed class SequentialLipAnimator : ILipAnimator
    {
        private readonly ILipMorpher lipMorpher;

        private CancellationTokenSource? cancellationTokenSource;

        public SequentialLipAnimator(ILipMorpher lipMorpher)
        {
            this.lipMorpher = lipMorpher;
        }

        public async UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> lipAnimationFrames,
            CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            
            lipMorpher.Reset();

            foreach (var lipAnimationFrame in lipAnimationFrames)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                lipMorpher.MorphInto(lipAnimationFrame.sample);

                var result = await RelentUniTask.Delay(
                    TimeSpan.FromSeconds(lipAnimationFrame.durationSeconds),
                    cancellationToken: cancellationTokenSource.Token);
                if (result.Failure)
                {
                    break;
                }
            }
            
            lipMorpher.Reset();

            cancellationTokenSource = null;
        }
    }
}