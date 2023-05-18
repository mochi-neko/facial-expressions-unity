#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// A generator of <see cref="LipAnimationFrame"/> by audio volume of <see cref="AudioSource"/>.
    /// </summary>
    public sealed class VolumeBasedLipAnimationFrameGenerator
    {
        private readonly AudioSource audioSource;
        private readonly float[] samples;

        public VolumeBasedLipAnimationFrameGenerator(
            AudioSource audioSource,
            int samplesCount = 1024)
        {
            this.audioSource = audioSource;
            this.samples = new float[samplesCount];
        }

        /// <summary>
        /// Samples a lip animation frame by current audio volume.
        /// </summary>
        /// <param name="viseme"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public IEnumerable<LipAnimationFrame> SampleFrame(Viseme viseme, float duration)
        {
            audioSource.GetOutputData(samples, channel: 0);

            var volume = CalculateVolume(samples);

            return new[]
            {
                new LipAnimationFrame(
                    new LipSample(viseme, volume),
                    duration
                )
            };
        }

        private static float CalculateVolume(float[] samples)
        {
            var sum = 0f;
            foreach (var sample in samples)
            {
                sum += sample * sample;
            }

            return Mathf.Sqrt(sum / samples.Length); // Root mean square
        }
    }
}