#nullable enable
using System;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Defines an animator of emotion.
    /// </summary>
    /// <typeparam name="TEmotion"></typeparam>
    public interface IEmotionAnimator<TEmotion>
        where TEmotion : Enum
    {
        /// <summary>
        /// Emotes.
        /// </summary>
        /// <param name="sample"></param>
        void Emote(EmotionSample<TEmotion> sample);
        
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