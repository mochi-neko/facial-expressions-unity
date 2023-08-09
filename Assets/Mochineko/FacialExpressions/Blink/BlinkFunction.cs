#nullable enable
using System;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Provides blink functions.
    /// </summary>
    public static class BlinkFunction
    {
        /// <summary>
        /// Weight of blink approximated by linear function.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        public static float LinearWeight(float t, float tc)
            => t <= tc
                ? LinearClosingWeight(t, tc)
                : LinearOpeningWeight(t, tc);

        /// <summary>
        /// Weight of closing blink approximated by linear function.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float LinearClosingWeight(float t, float tc)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be between 0 and 1.");
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tc), tc,
                    "tc must be between 0 and 1.");
            }

            if (t > tc)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be less than tc.");
            }

            return t / tc;
        }

        /// <summary>
        /// Weight of opening blink approximated by linear function.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float LinearOpeningWeight(float t, float tc)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be between 0 and 1.");
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tc), tc,
                    "tc must be between 0 and 1.");
            }

            if (t < tc)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be greater than tc.");
            }

            return (1f - t) / (1f - tc);
        }

        /// <summary>
        /// Weight of blink approximated by exponential and quadratic functions.
        /// </summary>
        /// <param name="t">Parameter</param>
        /// <param name="tc">Parameter at closing</param>
        /// <param name="beta">Strength of closing</param>
        /// <param name="a">Strength of opening</param>
        /// <returns></returns>
        public static float ApproximatedWeight(float t, float tc, float beta, float a)
            => t <= tc
                ? ApproximatedClosingWeight(t, tc, beta)
                : ApproximatedOpeningWeight(t, tc, a);

        /// <summary>
        /// Weight of closing blink approximated by exponential and quadratic functions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tc"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float ApproximatedClosingWeight(float t, float tc, float beta)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be between 0 and 1.");
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tc), tc,
                    "tc must be between 0 and 1.");
            }

            if (t > tc)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be less than tc.");
            }

            if (beta <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(beta), beta,
                    "beta must be greater than 0.");
            }

            // f(t) = \frac{e^{\beta t} - 1}{e^{\beta t_c} - 1} \ & \ (0 \le t \le t_c)
            return (Mathf.Exp(beta * t) - 1f) / (Mathf.Exp(beta * tc) - 1f);
        }

        /// <summary>
        /// Weight of opening blink approximated by exponential and quadratic functions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="tc"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static float ApproximatedOpeningWeight(float t, float tc, float a)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be between 0 and 1.");
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tc), tc,
                    "tc must be between 0 and 1.");
            }

            if (t < tc)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(t), t,
                    "t must be greater than tc.");
            }

            if (a is < -1f or > 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(a), a,
                    "a must be between -1 and 1.");
            }

            // f(t) = - a (t - t_c)(1 - t) + \frac{1 - t}{1 - t_c} \ & \ (t_c \le t \le 1)
            return -a * (t - tc) * (1f - t) + (1f - t) / (1f - tc);
        }
    }
}
