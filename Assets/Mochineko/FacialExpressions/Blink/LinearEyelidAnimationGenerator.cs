#nullable enable
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A generator of <see cref="EyelidAnimationFrame"/> for linear eyelid animation.
    /// </summary>
    public static class LinearEyelidAnimationGenerator
    {
        /// <summary>
        /// Generates a collection of <see cref="EyelidAnimationFrame"/> for some blinking.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <param name="blinkCount"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="closingRate"></param>
        /// <param name="minDurationSeconds"></param>
        /// <param name="maxDurationSeconds"></param>
        /// <param name="minIntervalSeconds"></param>
        /// <param name="maxIntervalSeconds"></param>
        /// <returns></returns>
        public static IEnumerable<EyelidAnimationFrame> Generate(
            Eyelid eyelid,
            int blinkCount,
            int framesPerSecond = 60, float closingRate = 0.2f,
            float minDurationSeconds = 0.05f, float maxDurationSeconds = 0.2f,
            float minIntervalSeconds = 0.1f, float maxIntervalSeconds = 6f)
        {
            var frames = new List<EyelidAnimationFrame>();

            for (var i = 0; i < blinkCount; i++)
            {
                var duration = Random.Range(minDurationSeconds, maxDurationSeconds);
                frames.AddRange(GenerateBlinkAnimationFrames(
                    eyelid,
                    framesPerSecond,
                    duration,
                    closingRate));

                var interval = Random.Range(minIntervalSeconds, maxIntervalSeconds);
                frames.Add(new EyelidAnimationFrame(
                    new EyelidSample(eyelid, weight: 0f),
                    interval));
            }

            return frames;
        }

        /// <summary>
        /// Generates a collection of <see cref="EyelidAnimationFrame"/> for one blinking.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="duration"></param>
        /// <param name="closingRate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<EyelidAnimationFrame> GenerateBlinkAnimationFrames(
            Eyelid eyelid,
            int framesPerSecond, float duration, float closingRate)
        {
            if (framesPerSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(framesPerSecond));
            }

            if (duration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (closingRate is <= 0f or >= 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(closingRate));
            }

            int frameCount = (int)(duration * framesPerSecond) + 1;
            var frames = new EyelidAnimationFrame[frameCount];
            var t = 0f;
            var dt = duration / frameCount;

            for (var i = 0; i < frameCount - 1; i++)
            {
                var weight = BlinkFunction
                    .LinearWeight(t / duration, closingRate);

                frames[i] = new EyelidAnimationFrame(
                    new EyelidSample(eyelid, weight),
                    dt);

                t += dt;
            }

            frames[frameCount - 1] = new EyelidAnimationFrame(
                new EyelidSample(eyelid, weight: 0f),
                duration - (t - dt));

            return frames;
        }
    }
}