#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.Emotion;
using UniVRM10;

namespace Mochineko.FacialExpressions.Samples
{
    /// <summary>
    /// A sample of emotion morpher for VRM models.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class VRMEmotionMorpher : IEmotionMorpher<BasicEmotion>
    {
        private readonly Vrm10RuntimeExpression expression;

        private static readonly IReadOnlyDictionary<BasicEmotion, ExpressionKey> KeyMap
            = new Dictionary<BasicEmotion, ExpressionKey>
            {
                [BasicEmotion.Neutral] = ExpressionKey.Neutral,
                [BasicEmotion.Happy] = ExpressionKey.Happy,
                [BasicEmotion.Sad] = ExpressionKey.Sad,
                [BasicEmotion.Angry] = ExpressionKey.Angry,
                [BasicEmotion.Fearful] = ExpressionKey.Neutral,
                [BasicEmotion.Surprised] = ExpressionKey.Surprised,
                [BasicEmotion.Disgusted] = ExpressionKey.Neutral,
            };

        public VRMEmotionMorpher(Vrm10RuntimeExpression expression)
        {
            this.expression = expression;
        }

        public void MorphInto(EmotionSample<BasicEmotion> sample)
        {
            if (KeyMap.TryGetValue(sample.emotion, out var key))
            {
                expression.SetWeight(key, sample.weight);
            }
        }

        public float GetWeightOf(BasicEmotion basicEmotion)
        {
            if (KeyMap.TryGetValue(basicEmotion, out var key))
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
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Neutral, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Happy, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Sad, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Angry, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Fearful, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Surprised, weight: 0f));
            MorphInto(new EmotionSample<BasicEmotion>(BasicEmotion.Disgusted, weight: 0f));
        }
    }
}