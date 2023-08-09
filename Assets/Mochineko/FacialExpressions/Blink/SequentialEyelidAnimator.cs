#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A sequential eyelid animator that animates eyelid sequentially by frame collection.
    /// </summary>
    public sealed class SequentialEyelidAnimator : ISequentialEyelidAnimator
    {
        private readonly IEyelidMorpher morpher;

        /// <summary>
        /// Creates a new instance of <see cref="SequentialEyelidAnimator"/>.
        /// </summary>
        /// <param name="morpher">Target morpher.</param>
        public SequentialEyelidAnimator(IEyelidMorpher morpher)
        {
            this.morpher = morpher;
        }

        public async UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            CancellationToken cancellationToken)
        {
            morpher.Reset();

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var frame in frames)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    morpher.MorphInto(frame.sample);

                    var result = await RelentUniTask.Delay(
                        TimeSpan.FromSeconds(frame.durationSeconds),
                        delayTiming: PlayerLoopTiming.Update,
                        cancellationToken: cancellationToken);

                    // Cancelled
                    if (result.Failure)
                    {
                        break;
                    }
                }
            }

            morpher.Reset();
        }
    }
}
