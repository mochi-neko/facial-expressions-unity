#nullable enable
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mochineko.FacialExpressions.Blink
{
    public static class RandomFunction
    {
        public static float GaussianRandomInRange(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min));
            }

            return Mathf.Clamp(
                GaussianRandom((min + max) / 2f, (max - min) / 6f),
                min, max);
        }

        public static float GaussianRandom(float mu, float sigma)
        {
            var u1 = Random.value;
            var u2 = Random.value;

            var z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);

            return mu + sigma * z;
        }
    }
}