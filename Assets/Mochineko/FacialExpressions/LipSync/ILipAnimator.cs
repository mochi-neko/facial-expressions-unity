#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Defines an object that animates lip by a collection of <see cref="LipAnimationFrame"/>.
    /// </summary>
    public interface ILipAnimator
    {
        /// <summary>
        /// Animates lip by a collection of <see cref="LipAnimationFrame"/>.
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
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