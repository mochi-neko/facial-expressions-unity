#nullable enable
using System;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Defines an animator of emotion per game engine frame.
    /// </summary>
    /// <typeparam name="TEmotion"></typeparam>
    public interface IFramewiseEmotionAnimator<TEmotion>
        where TEmotion : Enum
    {
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
