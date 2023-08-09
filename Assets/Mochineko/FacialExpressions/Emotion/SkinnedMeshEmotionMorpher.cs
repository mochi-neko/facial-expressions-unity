#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// A simple implementation of <see cref="IEmotionMorpher{TEmotion}"/> for <see cref="SkinnedMeshRenderer"/>.
    /// </summary>
    /// <typeparam name="TEmotion"></typeparam>
    public sealed class SkinnedMeshEmotionMorpher<TEmotion>
        : IEmotionMorpher<TEmotion>
        where TEmotion : Enum
    {
        private readonly SkinnedMeshRenderer skinnedMeshRenderer;
        private readonly IReadOnlyDictionary<TEmotion, int> indexMap;

        /// <summary>
        /// Creates a new instance of <see cref="SkinnedMeshEmotionMorpher{TEmotion}"/>.
        /// </summary>
        /// <param name="skinnedMeshRenderer">Target renderer.</param>
        /// <param name="indexMap">Map of viseme to blend shape index.</param>
        public SkinnedMeshEmotionMorpher(
            SkinnedMeshRenderer skinnedMeshRenderer,
            IReadOnlyDictionary<TEmotion, int> indexMap)
        {
            this.skinnedMeshRenderer = skinnedMeshRenderer;
            this.indexMap = indexMap;
        }

        public void MorphInto(EmotionSample<TEmotion> sample)
        {
            if (indexMap.TryGetValue(sample.emotion, out var index))
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, sample.weight * 100f);
            }
        }

        public float GetWeightOf(TEmotion emotion)
        {
            if (indexMap.TryGetValue(emotion, out var index))
            {
                return skinnedMeshRenderer.GetBlendShapeWeight(index) / 100f;
            }
            else
            {
                return 0f;
            }
        }

        public void Reset()
        {
            foreach (var index in indexMap.Values)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, 0f);
            }
        }
    }
}
