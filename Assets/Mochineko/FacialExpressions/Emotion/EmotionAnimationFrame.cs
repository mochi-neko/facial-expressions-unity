#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Frame of emotion animation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EmotionAnimationFrame<TEmotion>
        : IEquatable<EmotionAnimationFrame<TEmotion>>
        where TEmotion : Enum
    {
        /// <summary>
        /// Sample of emotion morphing.
        /// </summary>
        public readonly EmotionSample<TEmotion> sample;
        /// <summary>
        /// Duration of this frame in seconds.
        /// </summary>
        public readonly float durationSeconds;

        public EmotionAnimationFrame(EmotionSample<TEmotion> sample, float durationSeconds)
        {
            if (durationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds));
            }

            this.sample = sample;
            this.durationSeconds = durationSeconds;
        }

        public bool Equals(EmotionAnimationFrame<TEmotion> other)
        {
            return sample.Equals(other.sample)
                   && durationSeconds.Equals(other.durationSeconds);
        }

        public override bool Equals(object? obj)
        {
            return obj is EmotionAnimationFrame<TEmotion> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(sample, durationSeconds);
        }
    }
}
