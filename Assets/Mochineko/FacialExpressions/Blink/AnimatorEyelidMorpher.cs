#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    public sealed class AnimatorEyelidMorpher : IEyelidMorpher
    {
        private readonly Animator animator;
        private readonly IReadOnlyDictionary<Eyelid, int> idMap;

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
            MorphInto(new EyelidSample(Eyelid.Both, 0f));
            MorphInto(new EyelidSample(Eyelid.Left, 0f));
            MorphInto(new EyelidSample(Eyelid.Right, 0f));
        }
    }
}