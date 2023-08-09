#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Defines an animator to animate lip sequentially by frame collection.
    /// </summary>
    public interface ISequentialLipAnimator
    {
        /// <summary>
        /// Animates lip sequentially by a collection of <see cref="LipAnimationFrame"/>.
        /// </summary>
        /// <param name="frames">Target frames.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken);
    }
}
