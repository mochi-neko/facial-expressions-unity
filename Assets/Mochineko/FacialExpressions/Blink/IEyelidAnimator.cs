#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Defines an animator of eyelids.
    /// </summary>
    public interface IEyelidAnimator
    {
        /// <summary>
        /// Animates eyelid by a collection of <see cref="EyelidAnimationFrame"/>.
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="loop"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            bool loop,
            CancellationToken cancellationToken);

        /// <summary>
        /// Updates animation.
        /// </summary>
        void Update();
        
        /// <summary>
        /// Resets all morphing to default.
        /// </summary>
        void Reset();
    }
}