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

        /// <summary>
        /// Creates a new instance of <see cref="EmotionAnimationFrame{TEmotion}"/>.
        /// </summary>
        /// <param name="sample">Sample of emotion morphing.</param>
        /// <param name="durationSeconds">Duration of this frame in seconds.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EmotionAnimationFrame(EmotionSample<TEmotion> sample, float durationSeconds)
        {
            if (durationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(durationSeconds), durationSeconds,
                    "Duration must be greater than or equal to 0.");
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
