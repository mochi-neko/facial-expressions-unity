#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Loops animation of lip for any <see cref="ISequentialLipAnimator"/>.
    /// </summary>
    public sealed class LoopLipAnimator : IDisposable
    {
        private readonly ISequentialLipAnimator animator;
        private readonly IEnumerable<LipAnimationFrame> frames;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        /// <summary>
        /// Creates a new instance of <see cref="LoopLipAnimator"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="frames">Target frames.</param>
        public LoopLipAnimator(
            ISequentialLipAnimator animator,
            IEnumerable<LipAnimationFrame> frames)
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
