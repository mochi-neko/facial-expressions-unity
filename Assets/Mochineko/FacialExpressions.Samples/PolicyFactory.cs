#nullable enable
using System;
using System.IO;
using Mochineko.Relent.Resilience;
using Mochineko.Relent.Resilience.Bulkhead;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using Mochineko.VOICEVOX_API.QueryCreation;

namespace Mochineko.FacialExpressions.Samples
{
    internal static class PolicyFactory
    {
        private const float TotalTimeoutSeconds = 60f;
        private const float EachTimeoutSeconds = 30f;
        private const int MaxRetryCount = 5;
        private const float RetryIntervalSeconds = 1f;
        private const int MaxParallelization = 1;
        
        public static IPolicy<Stream> BuildSynthesisPolicy()
        {
            var totalTimeoutPolicy = TimeoutFactory.Timeout<Stream>(
                timeout: TimeSpan.FromSeconds(TotalTimeoutSeconds));
            
            var retryPolicy = RetryFactory.RetryWithInterval<Stream>(
                MaxRetryCount,
                interval: TimeSpan.FromSeconds(RetryIntervalSeconds));

            var eachTimeoutPolicy = TimeoutFactory.Timeout<Stream>(
                timeout: TimeSpan.FromSeconds(EachTimeoutSeconds));

            var bulkheadPolicy = BulkheadFactory.Bulkhead<Stream>(
                MaxParallelization);

            return totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(eachTimeoutPolicy)
                .Wrap(bulkheadPolicy);
        }
        
        public static IPolicy<AudioQuery> BuildQueryCreationPolicy()
        {
            var totalTimeoutPolicy = TimeoutFactory.Timeout<AudioQuery>(
                timeout: TimeSpan.FromSeconds(TotalTimeoutSeconds));
            
            var retryPolicy = RetryFactory.RetryWithInterval<AudioQuery>(
                MaxRetryCount,
                interval: TimeSpan.FromSeconds(RetryIntervalSeconds));

            var eachTimeoutPolicy = TimeoutFactory.Timeout<AudioQuery>(
                timeout: TimeSpan.FromSeconds(EachTimeoutSeconds));

            var bulkheadPolicy = BulkheadFactory.Bulkhead<AudioQuery>(
                MaxParallelization);

            return totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(eachTimeoutPolicy)
                .Wrap(bulkheadPolicy);
        }
    }
}