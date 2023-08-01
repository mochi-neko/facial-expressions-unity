#nullable enable
using System;
using System.Collections.Generic;
using Mochineko.FacialExpressions.LipSync;
using UniRx;
using UnityEngine;

namespace Mochineko.FacialExpressions.Extensions.uLipSync
{
    public sealed class ULipSyncAnimator : IDisposable
    {
        private readonly FollowingLipAnimator followingLipAnimator;
        private readonly global::uLipSync.uLipSync uLipSync;
        private readonly IDisposable disposable;

        private static readonly IReadOnlyDictionary<string, Viseme> phonomeMap
            = new Dictionary<string, Viseme>
            {
                ["A"] = Viseme.aa,
                ["I"] = Viseme.ih,
                ["U"] = Viseme.ou,
                ["E"] = Viseme.E,
                ["O"] = Viseme.oh,
            };

        public ULipSyncAnimator(
            FollowingLipAnimator followingLipAnimator,
            global::uLipSync.uLipSync uLipSync,
            float volumeThreshold = 0.01f,
            bool verbose = false)
        {
            this.followingLipAnimator = followingLipAnimator;
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