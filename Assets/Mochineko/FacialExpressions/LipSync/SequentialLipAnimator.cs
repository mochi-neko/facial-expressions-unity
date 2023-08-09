#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// A sequential lip animator.
    /// </summary>
    public sealed class SequentialLipAnimator : ISequentialLipAnimator
    {
        private readonly ILipMorpher morpher;

        private CancellationTokenSource? animationCanceller;

        /// <summary>
        /// Creates a new instance of <see cref="SequentialLipAnimator"/>.
        /// </summary>
        /// <param name="morpher">Target morpher.</param>
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

                morpher.MorphInto(frame.sample);

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

            animationCanceller = null;
        }
    }
}
