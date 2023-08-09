#nullable enable
using System;
using System.Collections.Generic;
using Mochineko.FacialExpressions.Emotion;
using UniVRM10;

namespace Mochineko.FacialExpressions.Samples
{
    /// <summary>
    /// A sample of emotion morpher for VRM models.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class VRMEmotionMorpher<TEmotion> :
        IEmotionMorpher<TEmotion>
        where TEmotion: Enum
    {
        private readonly Vrm10RuntimeExpression expression;
        private readonly IReadOnlyDictionary<TEmotion, ExpressionKey> keyMap;

        /// <summary>
        /// Creates a new instance of <see cref="VRMEmotionMorpher"/>.
        /// </summary>
        /// <param name="expression">Target expression of VRM instance.</param>
        /// <param name="keyMap">Map of emotion to expression key.</param>
        public VRMEmotionMorpher(
            Vrm10RuntimeExpression expression,
            IReadOnlyDictionary<TEmotion, ExpressionKey> keyMap)
        {
            this.expression = expression;
            this.keyMap = keyMap;
        }

        public void MorphInto(EmotionSample<TEmotion> sample)
        {
            if (keyMap.TryGetValue(sample.emotion, out var key))
            {
                expression.SetWeight(key, sample.weight);
            }
        }

        public float GetWeightOf(TEmotion emotion)
        {
            if (keyMap.TryGetValue(emotion, out var key))
            {
                return expression.GetWeight(key);
            }
            else
            {
                return 0f;
            }
        }

        public void Reset()
        {
            expression.SetWeight(ExpressionKey.Neutral, 0f);
            expression.SetWeight(ExpressionKey.Happy, 0f);
            expression.SetWeight(ExpressionKey.Angry, 0f);
            expression.SetWeight(ExpressionKey.Sad, 0f);
            expression.SetWeight(ExpressionKey.Relaxed, 0f);
            expression.SetWeight(ExpressionKey.Surprised, 0f);
        }
    }
}
