#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    public static class CompletionFunction
    {
        public static IEnumerable<LipAnimationFrame> CompleteCubic(
            Viseme viseme,
            float duration,
            float maxWeight = 1f,
            int framesPerSecond = 60)
        {
            var framesCount = (int)(duration * framesPerSecond) + 1;
            var frames = new LipAnimationFrame[framesCount];
            var dt = duration / framesCount;
            var t = 0f;

            for (var i = 0; i < framesCount; i++)
            {
                var phase = 2f * t / duration;
                if (phase <= 2f)
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            weight: maxWeight * Cubic(phase <= 1f
                                ? phase
                                : 2f - phase)),
                        dt);

                    t += dt;
                }
                else
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            weight: 0f),
                        duration - (t - dt));
                }
            }

            return frames;
        }

        public static IEnumerable<LipAnimationFrame> CompleteCubicIn(
            Viseme viseme,
            float duration,
            float maxWeight = 1f,
            int framesPerSecond = 60)
        {
            var framesCount = (int)(duration * framesPerSecond) + 1;
            var frames = new LipAnimationFrame[framesCount];
            var dt = duration / framesCount;
            var t = 0f;

            for (var i = 0; i < framesCount; i++)
            {
                var phase = t / duration;
                if (phase <= 1f)
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            maxWeight * Cubic(phase)),
                        dt);

                    t += dt;
                }
                else
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            maxWeight),
                        duration - (t - dt));
                }
            }

            return frames;
        }
        
        public static IEnumerable<LipAnimationFrame> CompleteCubicOut(
            Viseme viseme,
            float duration,
            float maxWeight = 1f,
            int framesPerSecond = 60)
        {
            var framesCount = (int)(duration * framesPerSecond) + 1;
            var frames = new LipAnimationFrame[framesCount];
            var dt = duration / framesCount;
            var t = 0f;

            for (var i = 0; i < framesCount; i++)
            {
                var phase = 1f - t / duration;
                if (phase >= 0f)
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            maxWeight * Cubic(phase)),
                        dt);

                    t += dt;
                }
                else
                {
                    frames[i] = new LipAnimationFrame(
                        new LipSample(
                            viseme,
                            weight: 0f),
                        duration - (t - dt));
                }
            }

            return frames;
        }

        public static float Cubic(float phase)
        {
            if (phase is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(phase));
            }

            return 3f * phase * phase - 2f * phase * phase * phase;
        }
    }
}