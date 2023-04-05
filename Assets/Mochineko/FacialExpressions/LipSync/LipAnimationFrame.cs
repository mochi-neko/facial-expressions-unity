#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Frame of lip animation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct LipAnimationFrame : IEquatable<LipAnimationFrame>
    {
        /// <summary>
        /// Sample of lip morphing.
        /// </summary>
        public readonly LipSample sample;
        /// <summary>
        /// Duration of this frame in seconds.
        /// </summary>
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