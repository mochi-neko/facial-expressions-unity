#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.Emotion;
using UnityEngine;
using UniVRM10;

namespace Mochineko.FacialExpressions.Samples
{
    /// <summary>
    /// A sample of emotion morpher for VRM models.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class VRMEmotionMorpher : IEmotionMorpher<Emotion.Emotion>
    {
        private readonly Vrm10RuntimeExpression expression;

        private static readonly IReadOnlyDictionary<Emotion.Emotion, ExpressionKey> KeyMap
            = new Dictionary<Emotion.Emotion, ExpressionKey>
            {
                [Emotion.Emotion.Neutral] = ExpressionKey.Neutral,
                [Emotion.Emotion.Happy] = ExpressionKey.Happy,
                [Emotion.Emotion.Sad] = ExpressionKey.Sad,
                [Emotion.Emotion.Angry] = ExpressionKey.Angry,
                [Emotion.Emotion.Fear] = ExpressionKey.Neutral,
                [Emotion.Emotion.Surprised] = ExpressionKey.Surprised,
                [Emotion.Emotion.Disgusted] = ExpressionKey.Neutral,
            };

        public VRMEmotionMorpher(Vrm10RuntimeExpression expression)
        {
            this.expression = expression;
        }

        public void MorphInto(EmotionSample<Emotion.Emotion> sample)
        {
            if (KeyMap.TryGetValue(sample.emotion, out var key))
            {
                expression.SetWeight(key, sample.weight);
            }
        }

        public float GetWeightOf(Emotion.Emotion emotion)
        {
            if (KeyMap.TryGetValue(emotion, out var key))
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
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Neutral, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Happy, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Sad, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Angry, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Fear, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Surprised, weight: 0f));
            MorphInto(new EmotionSample<Emotion.Emotion>(Emotion.Emotion.Disgusted, weight: 0f));
        }
    }
}