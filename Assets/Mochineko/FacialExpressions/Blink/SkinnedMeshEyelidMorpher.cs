#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A simple implementation of <see cref="IEyelidMorpher"/> for <see cref="skinnedMeshRenderer"/>.
    /// </summary>
    public sealed class SkinnedMeshEyelidMorpher : IEyelidMorpher
    {
        private readonly SkinnedMeshRenderer skinnedMeshRenderer;
        private readonly IReadOnlyDictionary<Eyelid, int> indexMap;
        private readonly bool separateBoth;

        /// <summary>
        /// Creates a new instance of <see cref="SkinnedMeshEyelidMorpher"/>.
        /// </summary>
        /// <param name="skinnedMeshRenderer">Target renderer.</param>
        /// <param name="indexMap">Map of eyelid and blend shape index.</param>
        /// <param name="separateBoth">Whether separate both eyelids blend shape.</param>
        public SkinnedMeshEyelidMorpher(
            SkinnedMeshRenderer skinnedMeshRenderer,
            IReadOnlyDictionary<Eyelid, int> indexMap,
            bool separateBoth = false)
        {
            this.skinnedMeshRenderer = skinnedMeshRenderer;
            this.indexMap = indexMap;
            this.separateBoth = separateBoth;
        }

        public void MorphInto(EyelidSample sample)
        {
            if (separateBoth && sample.eyelid == Eyelid.Both)
            {
                if (indexMap.TryGetValue(Eyelid.Left, out var rightIndex))
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(rightIndex, sample.weight * 100f);
                }
                if (indexMap.TryGetValue(Eyelid.Right, out var leftIndex))
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(leftIndex, sample.weight * 100f);
                }
            }
            else if (indexMap.TryGetValue(sample.eyelid, out var index))
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, sample.weight * 100f);
            }
        }

        public float GetWeightOf(Eyelid eyelid)
        {
            if (indexMap.TryGetValue(eyelid, out var index))
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
            foreach (var pair in indexMap)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(pair.Value, 0f);
            }
        }
    }
}
