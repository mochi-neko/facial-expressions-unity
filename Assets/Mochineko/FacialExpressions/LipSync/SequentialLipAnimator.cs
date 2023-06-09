#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// A lip animator that animates lip sequentially.
    /// </summary>
    public sealed class SequentialLipAnimator : ILipAnimator
    {
        private readonly ILipMorpher morpher;

        private LipSample currentSample = new(Viseme.sil, 0f);
        private CancellationTokenSource? animationCanceller;

        public SequentialLipAnimator(ILipMorpher morpher)
        {
            this.morpher = morpher;
        }

        public async UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken)
        {
            animationCanceller?.Cancel();
            animationCanceller = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            
            morpher.Reset();

            foreach (var frame in frames)
            {
                if (animationCanceller.IsCancellationRequested)
                {
                    break;
                }

                currentSample = frame.sample;

                var result = await RelentUniTask.Delay(
                    TimeSpan.FromSeconds(frame.durationSeconds),
                    cancellationToken: animationCanceller.Token);
                
                // Cancelled
                if (result.Failure)
                {
                    break;
                }
            }
            
            morpher.Reset();
            currentSample = new LipSample(Viseme.sil, 0f);

            animationCanceller = null;
        }

        public void Update()
        {
            morpher.MorphInto(currentSample);
        }

        public void Reset()
        {
            morpher.Reset();
        }
    }
}