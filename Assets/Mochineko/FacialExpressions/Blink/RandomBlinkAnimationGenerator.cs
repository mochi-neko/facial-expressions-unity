#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mochineko.FacialExpressions.Blink
{
    public static class RandomBlinkAnimationGenerator
    {
        public static IEnumerable<EyelidAnimationFrame> Generate(
            Eyelid eyelid,
            int blinkCount,
            int framesPerSecond = 60, float closingRate = 0.2f,
            float beta = 10f, float a = -1f,
            float minDurationSeconds = 0.05f, float maxDurationSeconds = 0.2f,
            float minIntervalSeconds = 0.1f, float maxIntervalSeconds = 6f)
        {
            var frames = new List<EyelidAnimationFrame>();

            var initialInterval = GaussianRandomInRange(
                minIntervalSeconds,
                maxIntervalSeconds);
            frames.Add(new EyelidAnimationFrame(
                new EyelidSample(eyelid, weight: 0f),
                initialInterval));

            for (var i = 0; i < blinkCount; i++)
            {
                var duration = GaussianRandomInRange(
                    minDurationSeconds,
                    maxDurationSeconds);
                frames.AddRange(GenerateAnimationFramesOfOneBlink(
                    eyelid,
                    framesPerSecond,
                    duration,
                    closingRate,
                    beta,
                    a));

                var interval = GaussianRandomInRange(
                    minIntervalSeconds,
                    maxIntervalSeconds);
                frames.Add(new EyelidAnimationFrame(
                    new EyelidSample(eyelid, weight: 0f),
                    interval));
            }

            return frames;
        }

        private static IEnumerable<EyelidAnimationFrame> GenerateAnimationFramesOfOneBlink(
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
            var tc = duration * closingRate;
            var dt = duration / frameCount;

            frames[0] = new EyelidAnimationFrame(
                new EyelidSample(eyelid, weight: 0f),
                dt);
            t += dt;

            for (var i = 1; i < frameCount - 1; i++)
            {
                var weight = t < tc
                    ? BlinkFunction.ApproximatedClosingWeight(t, tc, beta)
                    : BlinkFunction.ApproximatedOpeningWeight(t, tc, duration, a);

                frames[i] = new EyelidAnimationFrame(
                    new EyelidSample(eyelid, weight),
                    dt);

                t += dt;
            }

            frames[frameCount - 1] = new EyelidAnimationFrame(
                new EyelidSample(eyelid, weight: 0f),
                duration - t);

            return frames;
        }

        private static float GaussianRandomInRange(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min));
            }
            
            return GaussianRandom((min + max) / 2f, (max - min) / 6f);
        }

        private static float GaussianRandom(float mu, float sigma)
        {
            var u1 = Random.value;
            var u2 = Random.value;

            var z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);

            return mu + sigma * z;
        }
    }
}