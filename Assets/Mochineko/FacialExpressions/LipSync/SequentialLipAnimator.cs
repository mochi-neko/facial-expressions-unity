#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.LipSync
{
    public sealed class SequentialLipAnimator : ISequentialLipAnimator
    {
        private readonly ILipMorpher morpher;

        private CancellationTokenSource? cancellationTokenSource;

        public SequentialLipAnimator(ILipMorpher morpher)
        {
            this.morpher = morpher;
        }

        public async UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            
            morpher.Reset();

            foreach (var frame in frames)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                morpher.MorphInto(frame.sample);

                var result = await RelentUniTask.Delay(
                    TimeSpan.FromSeconds(frame.durationSeconds),
                    cancellationToken: cancellationTokenSource.Token);
                
                // Cancelled
                if (result.Failure)
                {
                    break;
                }
            }
            
            morpher.Reset();

            cancellationTokenSource = null;
        }
    }
}