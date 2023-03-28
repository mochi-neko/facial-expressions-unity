#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct LipSample : IEquatable<LipSample>
    {
        public LipSample(Viseme viseme, float weight)
        {
            if (weight is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
            }

            Viseme = viseme;
            Weight = weight;
        }

        public Viseme Viseme { get; }
        public float Weight { get; }

        public bool Equals(LipSample other)
        {
            return Viseme == other.Viseme && Weight.Equals(other.Weight);
        }

        public override bool Equals(object? obj)
        {
            return obj is LipSample other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Viseme, Weight);
        }
    }
}