#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A simple implementation of <see cref="IEyelidMorpher"/> for <see cref="Animator"/>.
    /// </summary>
    public sealed class AnimatorEyelidMorpher : IEyelidMorpher
    {
        private readonly Animator animator;
        private readonly IReadOnlyDictionary<Eyelid, int> idMap;

        /// <summary>
        /// Creates a new instance of <see cref="AnimatorEyelidMorpher"/>.
        /// </summary>
        /// <param name="animator">Target animator.</param>
        /// <param name="idMap">Map of eyelid to animator float key.</param>
        public AnimatorEyelidMorpher(
            Animator animator,
            IReadOnlyDictionary<Eyelid, int> idMap)
        {
            this.animator = animator;
            this.idMap = idMap;
        }

        public void MorphInto(EyelidSample sample)
        {
            if (idMap.TryGetValue(sample.eyelid, out var id))
            {
                animator.SetFloat(id, sample.weight);
            }
        }

        public float GetWeightOf(Eyelid eyelid)
        {
            if (idMap.TryGetValue(eyelid, out var id))
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
