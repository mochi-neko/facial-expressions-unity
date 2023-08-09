#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Defines an animator to animate eyelid sequentially by frame collection.
    /// </summary>
    public interface ISequentialEyelidAnimator
    {
        /// <summary>
        /// Animates eyelid by a collection of <see cref="EyelidAnimationFrame"/>.
        /// </summary>
        /// <param name="frames">Target frames.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            CancellationToken cancellationToken);
    }
}
