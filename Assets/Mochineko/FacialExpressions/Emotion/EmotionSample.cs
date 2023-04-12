#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// Sample of emotion morphing.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct EmotionSample<TEmotion>
        : IEquatable<EmotionSample<TEmotion>>
        where TEmotion : Enum
    {
        /// <summary>
        /// Target emotion.
        /// </summary>
        public readonly TEmotion emotion;

        /// <summary>
        /// Weight of morphing.
        /// </summary>
        public readonly float weight;

        public EmotionSample(TEmotion emotion, float weight)
        {
            if (weight is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
            }

            this.emotion = emotion;
            this.weight = weight;
        }

        public bool Equals(EmotionSample<TEmotion> other)
        {
            return EqualityComparer<TEmotion>.Default.Equals(emotion, other.emotion)
                   && weight.Equals(other.weight);
        }

        public override bool Equals(object? obj)
        {
            return obj is EmotionSample<TEmotion> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(emotion, weight);
        }
    }
}