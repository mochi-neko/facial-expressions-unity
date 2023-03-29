#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct LipSample : IEquatable<LipSample>
    {
        public readonly Viseme viseme;
        public readonly float weight;
        
        public LipSample(Viseme viseme, float weight)
        {
            if (weight is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
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