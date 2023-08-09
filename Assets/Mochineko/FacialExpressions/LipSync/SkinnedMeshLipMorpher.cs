#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// An implementation of <see cref="ILipMorpher"/> to morph lip by <see cref="SkinnedMeshRenderer"/>.
    /// </summary>
    public sealed class SkinnedMeshLipMorpher : ILipMorpher
    {
        private readonly SkinnedMeshRenderer skinnedMeshRenderer;
        private readonly IReadOnlyDictionary<Viseme, int> indexMap;

        /// <summary>
        /// Creates a new instance of <see cref="SkinnedMeshLipMorpher"/>.
        /// </summary>
        /// <param name="skinnedMeshRenderer">Target renderer.</param>
        /// <param name="indexMap">Map of viseme to blend shape index.</param>
        public SkinnedMeshLipMorpher(
            SkinnedMeshRenderer skinnedMeshRenderer,
            IReadOnlyDictionary<Viseme, int> indexMap)
        {
            this.skinnedMeshRenderer = skinnedMeshRenderer;
            this.indexMap = indexMap;
        }

        public void MorphInto(LipSample sample)
        {
            if (indexMap.TryGetValue(sample.viseme, out var index))
            {
                skinnedMeshRenderer.SetBlendShapeWeight(index, sample.weight * 100f);
            }
            else if (sample.viseme is Viseme.sil)
            {
                Reset();
            }
        }

        public float GetWeightOf(Viseme viseme)
        {
            if (indexMap.TryGetValue(viseme, out var index))
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
