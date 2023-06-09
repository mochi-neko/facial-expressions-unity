#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.UniTask;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// A simple implementation of <see cref="IEyelidAnimator"/> that animates eyelid sequentially by frame collection.
    /// </summary>
    public sealed class SequentialEyelidAnimator : IEyelidAnimator
    {
        private readonly IEyelidMorpher morpher;
        
        private EyelidSample currentSample = new(Eyelid.Both, 0f);
        private CancellationTokenSource? cancellationTokenSource;

        public SequentialEyelidAnimator(IEyelidMorpher morpher)
        {
            this.morpher = morpher;
        }
        
        public async UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            bool loop,
            CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            
            morpher.Reset();
            currentSample = new EyelidSample(Eyelid.Both, 0f);

            while (loop && !cancellationTokenSource.IsCancellationRequested)
            {
                foreach (var frame in frames)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    
                    currentSample = frame.sample;

                    var result = await RelentUniTask.Delay(
                        TimeSpan.FromSeconds(frame.durationSeconds),
                        delayTiming: PlayerLoopTiming.Update,
                        cancellationToken: cancellationTokenSource.Token);

                    // Cancelled
                    if (result.Failure)
                    {
                        break;
                    }
                }
            }

            morpher.Reset();
            currentSample = new EyelidSample(Eyelid.Both, 0f);

            cancellationTokenSource = null;
        }

        public void Update()
        {
            morpher.MorphInto(currentSample);
        }

        public void Reset()
        {
            morpher.Reset();
        }
    }
}