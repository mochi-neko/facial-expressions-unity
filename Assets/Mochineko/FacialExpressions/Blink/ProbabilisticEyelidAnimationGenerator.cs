#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    public static class ProbabilisticEyelidAnimationGenerator
    {
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