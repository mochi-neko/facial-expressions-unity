#nullable enable
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Animates lip by volume of audio.
    /// </summary>
    public sealed class VolumeBasedLipAnimator
    {
        private readonly ILipMorpher morpher;
        private readonly Viseme viseme;
        private readonly AudioSource audioSource;
        private readonly float smoothTime;
        private readonly float volumeMultiplier;
        private readonly float[] audioSamples;

        private float currentVolume = 0f;
        private float velocity = 0f;

        public VolumeBasedLipAnimator(
            ILipMorpher morpher,
            Viseme viseme,
            AudioSource audioSource,
            float smoothTime = 0.1f,
            float volumeMultiplier = 1f,
            int samplesCount = 1024)
        {
            this.morpher = morpher;
            this.viseme = viseme;
            this.audioSource = audioSource;
            this.smoothTime = smoothTime;
            this.volumeMultiplier = volumeMultiplier;
            this.audioSamples = new float[samplesCount];
        }

        /// <summary>
        /// Updates lip animation by current audio voluem.
        /// </summary>
        public void Update()
        {
            morpher.MorphInto(GetSample());
        }

        /// <summary>
        /// Resets lip animation.
        /// </summary>
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