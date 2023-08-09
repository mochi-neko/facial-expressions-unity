#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// A sample of lip morphing.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct LipSample : IEquatable<LipSample>
    {
        /// <summary>
        /// Viseme to morph.
        /// </summary>
        public readonly Viseme viseme;

        /// <summary>
        /// Weight of morphing.
        /// </summary>
        public readonly float weight;

        /// <summary>
        /// Creates a new instance of <see cref="LipSample"/>.
        /// </summary>
        /// <param name="viseme">Viseme to morph.</param>
        /// <param name="weight">Weight of morphing.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public LipSample(Viseme viseme, float weight)
        {
            if (weight is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weight), weight,
                    "Weight must be between 0 and 1.");
            }

            this.viseme = viseme;
            this.weight = weight;
        }

        public bool Equals(LipSample other)
        {
            return viseme == other.viseme
                   && weight.Equals(other.weight);
        }

        public override bool Equals(object? obj)
        {
            return obj is LipSample other
                   && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                (int)viseme,
                weight);
        }
    }
}
