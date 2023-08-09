#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// A framewise lip animator that animates lip by following target weights.
    /// </summary>
    public sealed class FollowingLipAnimator : IFramewiseLipAnimator
    {
        private readonly ILipMorpher morpher;
        private readonly float dt;
        private readonly float initialFollowingVelocity;
        private readonly float followingTime;
        private readonly Dictionary<Viseme, float> targetWeights = new ();
        private readonly Dictionary<Viseme, float> followingVelocities = new();

        private CancellationTokenSource? animationCanceller;

        /// <summary>
        /// Creates a new instance of <see cref="FollowingLipAnimator"/>.
        /// </summary>
        /// <param name="morpher">Target morpher.</param>
        /// <param name="initialFollowingVelocity">Initial velocity of smooth damp.</param>
        /// <param name="followingTime">Time of smooth damp.</param>
        public FollowingLipAnimator(
            ILipMorpher morpher,
            float initialFollowingVelocity = 0.1f,
            float followingTime = 0.005f)
        {
            if (initialFollowingVelocity < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initialFollowingVelocity),
                    initialFollowingVelocity,
                    "Initial velocity must be greater than or equal to 0.");
            }

            if (followingTime < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(followingTime),
                    followingTime,
                    "Following time must be greater than or equal to 0.");
            }

            this.morpher = morpher;
            this.initialFollowingVelocity = initialFollowingVelocity;
            this.followingTime = followingTime;
        }

        public async UniTask SetTargetAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken)
        {
            animationCanceller?.Cancel();
            animationCanceller = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);

            foreach (var frame in frames)
            {
                if (animationCanceller.IsCancellationRequested)
                {
                    break;
                }

                if (targetWeights.ContainsKey(frame.sample.viseme))
                {
                    targetWeights[frame.sample.viseme] = frame.sample.weight;
                }
                else
                {
                    targetWeights.Add(frame.sample.viseme, frame.sample.weight);
                    followingVelocities.Add(frame.sample.viseme, initialFollowingVelocity);
                }

                var result = await RelentUniTask.Delay(
                    TimeSpan.FromSeconds(frame.durationSeconds),
                    cancellationToken: animationCanceller.Token);

                // Cancelled
                if (result.Failure)
                {
                    break;
                }
            }

            animationCanceller?.Cancel();
            animationCanceller = null;
        }

        public void SetTarget(LipSample sample)
        {
            if (targetWeights.ContainsKey(sample.viseme))
            {
                targetWeights[sample.viseme] = sample.weight;
            }
            else
            {
                targetWeights.Add(sample.viseme, sample.weight);
                followingVelocities.Add(sample.viseme, initialFollowingVelocity);
            }
        }

        public void Update()
        {
            foreach (var target in targetWeights)
            {
                var velocity = followingVelocities[target.Key];
                var smoothedWeight = Mathf.Clamp01(Mathf
                    .SmoothDamp(
                        current: morpher.GetWeightOf(target.Key),
                        target: target.Value,
                        ref velocity,
                        followingTime));
                followingVelocities[target.Key] = velocity;

                morpher.MorphInto(new LipSample(target.Key, smoothedWeight));
            }
        }

        public void Reset()
        {
            morpher.Reset();
        }
    }
}
