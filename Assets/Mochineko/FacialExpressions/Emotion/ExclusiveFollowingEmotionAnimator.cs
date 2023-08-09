#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mochineko.FacialExpressions.Emotion
{
    /// <summary>
    /// An emotion animator that animates emotion by following target weights exclusively.
    /// </summary>
    public sealed class ExclusiveFollowingEmotionAnimator<TEmotion>
        : IFramewiseEmotionAnimator<TEmotion>
        where TEmotion : Enum
    {
        private readonly IEmotionMorpher<TEmotion> morpher;
        private readonly float followingTime;
        private readonly Dictionary<TEmotion, EmotionSample<TEmotion>> targets = new();
        private readonly Dictionary<TEmotion, float> velocities = new();

        /// <summary>
        /// Creates a new instance of <see cref="ExclusiveFollowingEmotionAnimator{TEmotion}"/>.
        /// </summary>
        /// <param name="morpher">Target morpher.</param>
        /// <param name="followingTime">Following time to smooth dump.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ExclusiveFollowingEmotionAnimator(
            IEmotionMorpher<TEmotion> morpher,
            float followingTime)
        {
            if (followingTime <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(followingTime), followingTime,
                    "Following time must be greater than 0.");
            }

            this.morpher = morpher;
            this.followingTime = followingTime;
            this.morpher.Reset();
        }

        /// <summary>
        /// Emotes.
        /// </summary>
        /// <param name="sample"></param>
        public void Emote(EmotionSample<TEmotion> sample)
        {
            if (targets.ContainsKey(sample.emotion))
            {
                targets[sample.emotion] = sample;
                velocities[sample.emotion] = 0f;
            }
            else
            {
                targets.Add(sample.emotion, sample);
                velocities.Add(sample.emotion, 0f);
            }

            // Exclude other emotions
            var otherEmotions = new List<TEmotion>();
            foreach (var emotion in targets.Keys)
            {
                if (!emotion.Equals(sample.emotion))
                {
                    otherEmotions.Add(emotion);
                }
            }

            foreach (var other in otherEmotions)
            {
                targets[other] = new EmotionSample<TEmotion>(other, weight: 0f);
            }
        }

        public void Update()
        {
            foreach (var target in targets)
            {
                var velocity = velocities[target.Key];
                var smoothedWeight = Mathf.Clamp(
                    value: Mathf.SmoothDamp(
                        current: morpher.GetWeightOf(target.Key),
                        target: target.Value.weight,
                        ref velocity,
                        followingTime),
                    min: 0f,
                    max: target.Value.weight);
                velocities[target.Key] = velocity;

                morpher.MorphInto(new EmotionSample<TEmotion>(target.Key, smoothedWeight));
            }
        }

        public void Reset()
        {
            morpher.Reset();
        }
    }
}
