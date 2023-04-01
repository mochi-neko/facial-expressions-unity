#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    public sealed class SkinnedMeshEyelidMorpher : IEyelidMorpher
    {
        private readonly SkinnedMeshRenderer skinnedMeshRenderer;
        private readonly IReadOnlyDictionary<Eyelid, int> indexMap;

        public SkinnedMeshEyelidMorpher(
            SkinnedMeshRenderer skinnedMeshRenderer,
            IReadOnlyDictionary<Eyelid, int> indexMap)
        {
            this.skinnedMeshRenderer = skinnedMeshRenderer;
            this.indexMap = indexMap;
        }

        public void MorphInto(EyelidSample sample)
        {
            if (indexMap.TryGetValue(sample.eyelid, out var index))
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, sample.weight);
            }
        }

        public float GetWeightOf(Eyelid eyelid)
        {
            if (indexMap.TryGetValue(eyelid, out var index))
            {
                return skinnedMeshRenderer.GetBlendShapeWeight(index);
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