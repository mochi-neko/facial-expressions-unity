#nullable enable
using System;
using System.Collections.Generic;
using Mochineko.FacialExpressions.LipSync;
using UniRx;
using UnityEngine;

namespace Mochineko.FacialExpressions.Extensions.uLipSync
{
    /// <summary>
    /// An implementation of <see cref="IFramewiseLipAnimator"/> for <see cref="global::uLipSync.uLipSync"/>.
    /// </summary>
    public sealed class ULipSyncAnimator : IFramewiseLipAnimator, IDisposable
    {
        private readonly FollowingLipAnimator followingLipAnimator;
        private readonly global::uLipSync.uLipSync uLipSync;
        private readonly IReadOnlyDictionary<string, Viseme> phonomeMap;
        private readonly IDisposable disposable;

        /// <summary>
        /// Creates a new instance of <see cref="ULipSyncAnimator"/>.
        /// </summary>
        /// <param name="followingLipAnimator">Wrapped <see cref="FollowingLipAnimator"/>.</param>
        /// <param name="uLipSync">Target <see cref="global::uLipSync.uLipSync"/>.</param>
        /// <param name="phonomeMap">Map of phoneme to viseme.</param>
        /// <param name="volumeThreshold">Threshold of volume.</param>
        /// <param name="verbose">Whether to output log.</param>
        public ULipSyncAnimator(
            FollowingLipAnimator followingLipAnimator,
            global::uLipSync.uLipSync uLipSync,
            IReadOnlyDictionary<string, Viseme> phonomeMap,
            float volumeThreshold = 0.01f,
            bool verbose = false)
        {
            this.followingLipAnimator = followingLipAnimator;
            this.phonomeMap = phonomeMap;
            this.uLipSync = uLipSync;

            this.disposable = this.uLipSync
                .ObserveEveryValueChanged(lipSync => lipSync.result)
                .Subscribe(info =>
                {
                    if (verbose)
                    {
                        Debug.Log($"[FacialExpressions.uLipSync] Update lip sync: {info.phoneme}, {info.volume}");
                    }

                    if (phonomeMap.TryGetValue(info.phoneme, out var viseme) && info.volume > volumeThreshold)
                    {
                        SetTarget(new LipSample(viseme, weight: 1f));
                    }
                    else
                    {
                        SetDefault();
                    }
                });
        }

        public void Dispose()
        {
            Reset();
            disposable.Dispose();
        }

        public void Update()
        {
            followingLipAnimator.Update();
        }

        public void Reset()
        {
            followingLipAnimator.Reset();
        }

        private void SetTarget(LipSample sample)
        {
            foreach (var viseme in phonomeMap.Values)
            {
                if (viseme == sample.viseme)
                {
                    followingLipAnimator.SetTarget(sample);
                }
                else
                {
                    followingLipAnimator.SetTarget(new LipSample(viseme, weight: 0f));
                }
            }
        }

        private void SetDefault()
        {
            foreach (var viseme in phonomeMap.Values)
            {
                followingLipAnimator.SetTarget(new LipSample(viseme, weight: 0f));
            }
        }
    }
}
