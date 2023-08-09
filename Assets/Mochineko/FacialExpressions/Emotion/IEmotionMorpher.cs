#nullable enable
using System;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Defines a morpher of emotion to change shapes for each emotion.
    /// </summary>
    public interface IEmotionMorpher<TEmotion>
        where TEmotion : Enum
    {
        /// <summary>
        /// Morphs specified emotion into specified weight.
        /// </summary>
        /// <param name="sample">Target emotion sample.</param>
        void MorphInto(EmotionSample<TEmotion> sample);

        /// <summary>
        /// Gets current weight of specified emotion.
        /// </summary>
        /// <param name="emotion">Target emotion.</param>
        /// <returns></returns>
        float GetWeightOf(TEmotion emotion);

        /// <summary>
        /// Resets all morphing to default.
        /// </summary>
        void Reset();
    }
}
