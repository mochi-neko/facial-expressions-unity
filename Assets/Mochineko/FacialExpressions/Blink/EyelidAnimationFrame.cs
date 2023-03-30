#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.Blink
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EyelidAnimationFrame : IEquatable<EyelidAnimationFrame>
    {
        public readonly EyelidSample sample;
        public readonly float durationSeconds;

        public EyelidAnimationFrame(EyelidSample sample, float durationSeconds)
        {
            if (durationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds));
            }
            
            this.sample = sample;
            this.durationSeconds = durationSeconds;
        }

        public bool Equals(EyelidAnimationFrame other)
        {
            return sample.Equals(other.sample)
                   && durationSeconds.Equals(other.durationSeconds);
        }

        public override bool Equals(object? obj)
        {
            return obj is EyelidAnimationFrame other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(sample, durationSeconds);
        }
    }
}