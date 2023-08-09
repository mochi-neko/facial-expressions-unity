#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Defines an animator to animate emotion sequentially by frame collection.
    /// </summary>
    /// <typeparam name="TEmotion"></typeparam>
    public interface ISequentialEmotionAnimator<TEmotion>
        where TEmotion : Enum
    {
        /// <summary>
        /// Animates emotion sequentially by a collection of <see cref="EmotionAnimationFrame{TEmotion}"/>.
        /// </summary>
        /// <param name="frames">Target frames.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        UniTask AnimateAsync(
            IEnumerable<EmotionAnimationFrame<TEmotion>> frames,
            CancellationToken cancellationToken);
    }
}
