#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A generator of <see cref="EyelidAnimationFrame"/> for approximated eyelid animation.
    /// </summary>
    public static class ProbabilisticEyelidAnimationGenerator
    {
        /// <summary>
        /// Generates a collection of <see cref="EyelidAnimationFrame"/>
        ///  for approximated eyelid animation and harmonic interval.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <param name="blinkCount"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="closingRate"></param>
        /// <param name="beta"></param>
        /// <param name="a"></param>
        /// <param name="minDurationSeconds"></param>
        /// <param name="maxDurationSeconds"></param>
        /// <param name="minIntervalSeconds"></param>
        /// <param name="maxIntervalSeconds"></param>
        /// <param name="harmonicScale"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<EyelidAnimationFrame> Generate(
            Eyelid eyelid,
            int blinkCount,
            int framesPerSecond = 60, float closingRate = 0.2f,
            float beta = 10f, float a = 1f,
            float minDurationSeconds = 0.05f, float maxDurationSeconds = 0.2f,
            float minIntervalSeconds = 0.1f, float maxIntervalSeconds = 6f,
            float harmonicScale = 0.03f, float period = 3f)
        {
            var frames = new List<EyelidAnimationFrame>();

            for (var i = 0; i < blinkCount; i++)
            {
                var duration = RandomFunction.GaussianRandomInRange(
                    minDurationSeconds,
                    maxDurationSeconds);
                frames.AddRange(GenerateBlinkAnimationFrames(
                    eyelid,
                    framesPerSecond,
                    duration,
                    closingRate,
                    beta,
                    a));

                var interval = RandomFunction.GaussianRandomInRange(
                    minIntervalSeconds,
                    maxIntervalSeconds);
                frames.AddRange(GenerateHarmonicIntervalAnimationFrames(
                    eyelid,
                    framesPerSecond, interval,
                    harmonicScale, period));
            }

            return frames;
        }

        /// <summary>
        /// Generates a collection of <see cref="EyelidAnimationFrame"/>
        ///  for one blinking by approximated function.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="duration"></param>
        /// <param name="closingRate"></param>
        /// <param name="beta"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<EyelidAnimationFrame> GenerateBlinkAnimationFrames(
            Eyelid eyelid,
            int framesPerSecond, float duration, float closingRate,
            float beta, float a)
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
                    .ApproximatedWeight(t / duration, closingRate, beta, a);

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

        /// <summary>
        /// Generates a collection of <see cref="EyelidAnimationFrame"/>
        ///  for one interval by harmonic function.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <param name="framesPerSecond"></param>
        /// <param name="duration"></param>
        /// <param name="harmonicScale"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<EyelidAnimationFrame> GenerateHarmonicIntervalAnimationFrames(
            Eyelid eyelid,
            int framesPerSecond, float duration,
            float harmonicScale, float period)
        {
            int frameCount = (int)(duration * framesPerSecond) + 1;
            var frames = new EyelidAnimationFrame[frameCount];
            var t = 0f;
            var dt = duration / frameCount;

            for (var i = 0; i < frameCount - 1; i++)
            {
                var weight = harmonicScale * (Mathf.Sin(2f * Mathf.PI * t / period) + 1f) / 2f;

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