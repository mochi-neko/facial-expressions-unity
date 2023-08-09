#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Loops animation of eyelid for any <see cref="ISequentialEyelidAnimator"/>.
    /// </summary>
    public sealed class LoopEyelidAnimator : IDisposable
    {
        private readonly ISequentialEyelidAnimator animator;
        private readonly IEnumerable<EyelidAnimationFrame> frames;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        /// <summary>
        /// Creates a new instance of <see cref="LoopEyelidAnimator"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="frames">Target frames.</param>
        public LoopEyelidAnimator(
            ISequentialEyelidAnimator animator,
            IEnumerable<EyelidAnimationFrame> frames)
        {
            this.animator = animator;
            this.frames = frames;

            LoopAnimationAsync(cancellationTokenSource.Token)
                .Forget();
        }

        private async UniTask LoopAnimationAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await animator.AnimateAsync(frames, cancellationToken);

                await UniTask
                    .DelayFrame(delayFrameCount: 1, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
            }
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}
