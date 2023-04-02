#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;
using UnityEngine;

namespace Mochineko.FacialExpressions.LipSync
{
    public sealed class FollowingLipAnimator : ILipAnimator
    {
        private readonly ILipMorpher morpher;
        private readonly float dt;
        private readonly float initialFollowingVelocity;
        private readonly float followingTime;

        private CancellationTokenSource? animationCanceller;

        public FollowingLipAnimator(
            ILipMorpher morpher,
            int framesPerSecond = 60,
            float initialFollowingVelocity = 0.1f,
            float followingTime = 0.005f)
        {
            this.morpher = morpher;
            this.dt = 1f / framesPerSecond;
            this.initialFollowingVelocity = initialFollowingVelocity;
            this.followingTime = followingTime;
        }

        public async UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken)
        {
            animationCanceller?.Cancel();
            animationCanceller = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);

            var targetWeights = new Dictionary<Viseme, float>();
            var followingVelocities = new Dictionary<Viseme, float>();

            UpdateLoopAsync(targetWeights, followingVelocities, animationCanceller.Token)
                .Forget();

            morpher.Reset();

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

            animationCanceller.Cancel();

            morpher.Reset();

            animationCanceller = null;
        }

        private async UniTask UpdateLoopAsync(
            Dictionary<Viseme, float> targetWeights,
            Dictionary<Viseme, float> velocities,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var target in targetWeights)
                {
                    var velocity = velocities[target.Key];
                    var smoothedWeight = Mathf.Clamp01(Mathf
                        .SmoothDamp(
                            current: morpher.GetWeightOf(target.Key),
                            target: target.Value,
                            ref velocity,
                            followingTime));
                    velocities[target.Key] = velocity;

                    morpher.MorphInto(new LipSample(target.Key, smoothedWeight));
                }

                var result = await RelentUniTask.Delay(
                    TimeSpan.FromSeconds(dt),
                    cancellationToken: cancellationToken);

                // Cancelled
                if (result.Failure)
                {
                    break;
                }
            }
        }
    }
}