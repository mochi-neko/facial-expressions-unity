#nullable enable
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// An implementation of <see cref="IFramewiseLipAnimator"/> to animate lip by audio volume.
    /// </summary>
    public sealed class VolumeBasedLipAnimator : IFramewiseLipAnimator
    {
        private readonly ILipMorpher morpher;
        private readonly Viseme viseme;
        private readonly AudioSource audioSource;
        private readonly float smoothTime;
        private readonly float volumeMultiplier;
        private readonly float[] audioSamples;

        private float currentVolume = 0f;
        private float velocity = 0f;

        /// <summary>
        /// Creates a new instance of <see cref="VolumeBasedLipAnimator"/>.
        /// </summary>
        /// <param name="morpher">Target morpher.</param>
        /// <param name="viseme">Target viseme to morph.</param>
        /// <param name="audioSource">Audio source to get volume.</param>
        /// <param name="smoothTime">Smooth time of volume.</param>
        /// <param name="volumeMultiplier">Multiplier of volume.</param>
        /// <param name="samplesCount">Count of samples to get volume at each frame.</param>
        public VolumeBasedLipAnimator(
            ILipMorpher morpher,
            Viseme viseme,
            AudioSource audioSource,
            float smoothTime = 0.1f,
            float volumeMultiplier = 1f,
            int samplesCount = 1024)
        {
            if (smoothTime <= 0f)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(smoothTime), smoothTime,
                    "Smooth time must be greater than 0.");
            }

            if (volumeMultiplier <= 0f)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(volumeMultiplier), volumeMultiplier,
                    "Volume multiplier must be greater than 0.");
            }

            if (samplesCount <= 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(samplesCount), samplesCount,
                    "Samples count must be greater than 0.");
            }

            this.morpher = morpher;
            this.viseme = viseme;
            this.audioSource = audioSource;
            this.smoothTime = smoothTime;
            this.volumeMultiplier = volumeMultiplier;
            this.audioSamples = new float[samplesCount];
        }

        public void Update()
        {
            morpher.MorphInto(GetSample());
        }

        public void Reset()
        {
            morpher.Reset();
        }

        private LipSample GetSample()
        {
            audioSource.GetOutputData(audioSamples, channel: 0);

            var volume = CalculateVolume(audioSamples);

            currentVolume = Mathf.SmoothDamp(
                current: currentVolume,
                target: volume,
                currentVelocity: ref velocity,
                smoothTime: smoothTime
            );

            return new LipSample(
                viseme,
                Mathf.Clamp01(currentVolume * volumeMultiplier)
            );
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
