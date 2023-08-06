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
                skinnedMeshRenderer.SetBlendShapeWeight(index, sample.weight);
            }
            else if (sample.viseme is Viseme.sil)
            {
                Reset();
            }
        }

        public float GetWeightOf(Viseme viseme)
        {
            throw new System.NotImplementedException();
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