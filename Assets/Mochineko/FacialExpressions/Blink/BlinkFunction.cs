#nullable enable
using System;
using UnityEngine;

namespace Mochineko.FacialExpressions.Blink
{
    public static class BlinkFunction
    {
        public static float ApproximatedClosingWeight(float t, float tc, float beta)
        {
            if (t < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
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
        
        public static float ApproximatedOpeningWeight(float t, float tc, float to, float a)
        {
            if (t < tc)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (t > to)
            {
                throw new ArgumentOutOfRangeException(nameof(t));
            }

            if (tc > to)
            {
                throw new ArgumentOutOfRangeException(nameof(tc));
            }

            if (a is < -1f or > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }

            // f(t) = a (t - t_c)(t_o - t) + \frac{t_o - t}{t_o - t_c} \ & \ (t_c \le t \le t_o)
            return a * (t - tc) * (to - t) + (to - t) / (to - tc);
        }
    }
}