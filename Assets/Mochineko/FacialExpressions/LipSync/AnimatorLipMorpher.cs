#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    public sealed class AnimatorLipMorpher : ILipMorpher
    {
        private readonly Animator animator;
        private readonly IReadOnlyDictionary<Viseme, int> idMap;

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

        public void Reset()
        {
            MorphInto(new LipSample(Viseme.aa, 0f));
            MorphInto(new LipSample(Viseme.ih, 0f));
            MorphInto(new LipSample(Viseme.ou, 0f));
            MorphInto(new LipSample(Viseme.E, 0f));
            MorphInto(new LipSample(Viseme.oh, 0f));
        }
    }
}