#nullable enable
using System;

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Provides completion functions.
    /// </summary>
    public static class CompletionFunction
    {
        /// <summary>
        /// Cubic function from 0 to 1 with easing.
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float Cubic(float phase)
        {
            if (phase is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(phase), phase,
                    "Phase must be between 0 and 1.");
            }

            return 3f * phase * phase - 2f * phase * phase * phase;
        }
    }
}
