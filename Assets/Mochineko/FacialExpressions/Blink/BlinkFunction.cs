#nullable enable
using System;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    public static class BlinkFunction
    {
        public static float LinearWeight(float t, float tc)
            => t <= tc
                ? LinearClosingWeight(t, tc)
                : LinearOpeningWeight(t, tc);
        
        public static float LinearClosingWeight(float t, float tc)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(tc));
            }

            if (t > tc)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            return t / tc;
        }

        public static float LinearOpeningWeight(float t, float tc)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(tc));
            }

            if (t < tc)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            return (1f - t) / (1f - tc);
        }

        public static float ApproximatedWeight(float t, float tc, float beta, float a)
            => t <= tc
                ? ApproximatedClosingWeight(t, tc, beta)
                : ApproximatedOpeningWeight(t, tc, a);

        public static float ApproximatedClosingWeight(float t, float tc, float beta)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(tc));
            }

            if (t > tc)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (beta <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(beta));
            }

            // f(t) = \frac{e^{\beta t} - 1}{e^{\beta t_c} - 1} \ & \ (0 \le t \le t_c)
            return (Mathf.Exp(beta * t) - 1f) / (Mathf.Exp(beta * tc) - 1f);
        }

        public static float ApproximatedOpeningWeight(float t, float tc, float a)
        {
            if (t is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (tc is < 0f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(tc));
            }

            if (t < tc)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (a is < -1f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }

            // f(t) = - a (t - t_c)(1 - t) + \frac{1 - t}{1 - t_c} \ & \ (t_c \le t \le 1)
            return -a * (t - tc) * (1f - t) + (1f - t) / (1f - tc);
        }
    }
}