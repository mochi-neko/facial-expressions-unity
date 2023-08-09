#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// An implementation of <see cref="ILipMorpher"/> to morph lip by <see cref="Animator"/>.
    /// </summary>
    public sealed class AnimatorLipMorpher : ILipMorpher
    {
        private readonly Animator animator;
        private readonly IReadOnlyDictionary<Viseme, int> idMap;

        /// <summary>
        /// Creates a new instance of <see cref="AnimatorLipMorpher"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="idMap">Map of viseme to animator float key.</param>
        public AnimatorLipMorpher(
            Animator animator,
            IReadOnlyDictionary<Viseme, int> idMap)
        {
            this.animator = animator;
            this.idMap = idMap;
        }

        public void MorphInto(LipSample sample)
        {
            if (idMap.TryGetValue(sample.viseme, out var id))
            {
                animator.SetFloat(id, sample.weight);
            }
            else if (sample.viseme is Viseme.sil or Viseme.nn)
            {
                Reset();
            }
        }

        public float GetWeightOf(Viseme viseme)
        {
            if (idMap.TryGetValue(viseme, out var id))
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
            foreach (var pair in idMap)
            {
                animator.SetFloat(pair.Value, 0f);
            }
        }
    }
}
