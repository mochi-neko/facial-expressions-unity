#nullable enable
using System.Collections.Generic;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Composition of some <see cref="IEyelidMorpher"/>s.
    /// </summary>
    public sealed class CompositeEyelidMorpher : IEyelidMorpher
    {
        private readonly IReadOnlyList<IEyelidMorpher> morphers;

        /// <summary>
        /// Creates a new instance of <see cref="CompositeEyelidMorpher"/>.
        /// </summary>
        /// <param name="morphers">Composited morphers.</param>
        public CompositeEyelidMorpher(IReadOnlyList<IEyelidMorpher> morphers)
        {
            this.morphers = morphers;
        }

        void IEyelidMorpher.MorphInto(EyelidSample sample)
        {
            foreach (var morpher in morphers)
            {
                morpher.MorphInto(sample);
            }
        }

        float IEyelidMorpher.GetWeightOf(Eyelid eyelid)
        {
            return morphers[0].GetWeightOf(eyelid);
        }

        void IEyelidMorpher.Reset()
        {
            foreach (var morpher in morphers)
            {
                morpher.Reset();
            }
        }
    }
}
