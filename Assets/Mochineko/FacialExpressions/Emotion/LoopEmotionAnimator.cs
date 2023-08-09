#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Loops animation of eyelid for any <see cref="ISequentialEmotionAnimator{TEmotion}"/>.
    /// </summary>
    public sealed class LoopEmotionAnimator<TEmotion>
        : IDisposable
        where TEmotion : Enum
    {
        private readonly ISequentialEmotionAnimator<TEmotion> animator;
        private readonly IEnumerable<EmotionAnimationFrame<TEmotion>> frames;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        /// <summary>
        /// Creates a new instance of <see cref="LoopEmotionAnimator"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="frames">Target frames.</param>
        public LoopEmotionAnimator(
            ISequentialEmotionAnimator<TEmotion> animator,
            IEnumerable<EmotionAnimationFrame<TEmotion>> frames)
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
