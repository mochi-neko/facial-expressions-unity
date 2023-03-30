#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.LipSync
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct LipAnimationFrame : IEquatable<LipAnimationFrame>
    {
        public readonly LipSample sample;
        public readonly float durationSeconds;

        public LipAnimationFrame(LipSample sample, float durationSeconds)
        {
            if (durationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds));
            }
            
            this.sample = sample;
            this.durationSeconds = durationSeconds;
        }

        public bool Equals(LipAnimationFrame other)
        {
            return sample.Equals(other.sample)
                   && durationSeconds.Equals(other.durationSeconds);
        }

        public override bool Equals(object? obj)
        {
            return obj is LipAnimationFrame other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(sample, durationSeconds);
        }
    }
}