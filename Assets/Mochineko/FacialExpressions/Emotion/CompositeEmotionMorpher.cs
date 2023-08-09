#nullable enable
using System;
using System.Collections.Generic;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Composition of some <see cref="Mochineko.FacialExpressions.Emotion.IEmotionMorpher{TEmotion}"/>s.
    /// </summary>
    /// <typeparam name="TEmotion"></typeparam>
    public sealed class CompositeEmotionMorpher<TEmotion>
        : IEmotionMorpher<TEmotion>
        where TEmotion : Enum
    {
        private readonly IReadOnlyList<IEmotionMorpher<TEmotion>> morphers;

        /// <summary>
        /// Creates a new instance of <see cref="Mochineko.FacialExpressions.Emotion.CompositeEmotionMorpher{TEmotion}"/>.
        /// </summary>
        /// <param name="morphers">Composited morphers.</param>
        public CompositeEmotionMorpher(IReadOnlyList<IEmotionMorpher<TEmotion>> morphers)
        {
            this.morphers = morphers;
        }

        void IEmotionMorpher<TEmotion>.MorphInto(EmotionSample<TEmotion> sample)
        {
            foreach (var morpher in morphers)
            {
                morpher.MorphInto(sample);
            }
        }

        float IEmotionMorpher<TEmotion>.GetWeightOf(TEmotion emotion)
        {
            return morphers[0].GetWeightOf(emotion);
        }

        void IEmotionMorpher<TEmotion>.Reset()
        {
            foreach (var morpher in morphers)
            {
                morpher.Reset();
            }
        }
    }
}
