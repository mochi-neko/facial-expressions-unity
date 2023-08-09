#nullable enable
using System.Collections.Generic;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Composition of some <see cref="Mochineko.FacialExpressions.LipSync.ILipMorpher"/>s.
    /// </summary>
    public sealed class CompositeLipMorpher : ILipMorpher
    {
        private readonly IReadOnlyList<ILipMorpher> morphers;

        /// <summary>
        /// Creates a new instance of <see cref="Mochineko.FacialExpressions.LipSync.CompositeLipMorpher"/>.
        /// </summary>
        /// <param name="morphers">Composited morphers.</param>
        public CompositeLipMorpher(IReadOnlyList<ILipMorpher> morphers)
        {
            this.morphers = morphers;
        }

        void ILipMorpher.MorphInto(LipSample sample)
        {
            foreach (var morpher in morphers)
            {
                morpher.MorphInto(sample);
            }
        }

        float ILipMorpher.GetWeightOf(Viseme viseme)
        {
            return morphers[0].GetWeightOf(viseme);
        }

        void ILipMorpher.Reset()
        {
            foreach (var morpher in morphers)
            {
                morpher.Reset();
            }
        }
    }
}
