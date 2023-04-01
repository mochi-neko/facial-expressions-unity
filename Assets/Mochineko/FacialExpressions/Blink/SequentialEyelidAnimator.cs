#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.Blink
{
    public sealed class SequentialEyelidAnimator : IEyelidAnimator
    {
        private readonly IEyelidMorpher morpher;

        private CancellationTokenSource? cancellationTokenSource;

        public SequentialEyelidAnimator(IEyelidMorpher morpher)
        {
            this.morpher = morpher;
        }
        
        public async UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            bool loop,
            CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            
            morpher.Reset();

            while (loop && !cancellationTokenSource.IsCancellationRequested)
            {
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
            }

            morpher.Reset();

            cancellationTokenSource = null;
        }
    }
}