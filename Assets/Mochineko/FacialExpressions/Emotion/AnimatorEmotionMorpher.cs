#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// A simple implementation of <see cref="IEmotionMorpher{TEmotion}"/> for <see cref="Animator"/>.
    /// </summary>
    public sealed class AnimatorEmotionMorpher<TEmotion>
        : IEmotionMorpher<TEmotion>
        where TEmotion : Enum
    {
        private readonly Animator animator;
        private readonly IReadOnlyDictionary<TEmotion, int> idMap;

        /// <summary>
        /// Creates a new instance of <see cref="AnimatorEmotionMorpher{TEmotion}"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="idMap">Map of eyelid to animator float key.</param>
        public AnimatorEmotionMorpher(
            Animator animator,
            IReadOnlyDictionary<TEmotion, int> idMap)
        {
            this.animator = animator;
            this.idMap = idMap;
        }

        public void MorphInto(EmotionSample<TEmotion> sample)
        {
            if (idMap.TryGetValue(sample.emotion, out var id))
            {
                animator.SetFloat(id, sample.weight);
            }
        }

        public float GetWeightOf(TEmotion emotion)
        {
            if (idMap.TryGetValue(emotion, out var id))
            {
                return animator.GetFloat(id);
            }
            else
            {
                return 0f;
            }
        }

        public void Reset()
        {
            foreach (var id in idMap.Values)
            {
                animator.SetFloat(id, 0f);
            }
        }
    }
}
