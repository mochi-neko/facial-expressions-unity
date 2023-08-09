#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.Blink;
using UniVRM10;

namespace Mochineko.FacialExpressions.Extensions.VRM
{
    /// <summary>
    /// An eyelid morpher for VRM models.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class VRMEyelidMorpher : IEyelidMorpher
    {
        private readonly Vrm10RuntimeExpression expression;

        private static readonly IReadOnlyDictionary<Eyelid, ExpressionKey> KeyMap
            = new Dictionary<Eyelid, ExpressionKey>
            {
                [Eyelid.Both] = ExpressionKey.Blink,
                [Eyelid.Left] = ExpressionKey.BlinkLeft,
                [Eyelid.Right] = ExpressionKey.BlinkRight,
            };

        /// <summary>
        /// Create an eyelid morpher for VRM models.
        /// </summary>
        /// <param name="expression">Target expression of VRM instance.</param>
        public VRMEyelidMorpher(Vrm10RuntimeExpression expression)
        {
            this.expression = expression;
        }

        public void MorphInto(EyelidSample sample)
        {
            if (KeyMap.TryGetValue(sample.eyelid, out var key))
            {
                expression.SetWeight(key, sample.weight);
            }
        }

        public float GetWeightOf(Eyelid eyelid)
        {
            if (KeyMap.TryGetValue(eyelid, out var key))
            {
                return expression.GetWeight(key);
            }
            else
            {
                return 0f;
            }
        }

        public void Reset()
        {
            expression.SetWeight(ExpressionKey.BlinkLeft, 0f);
            expression.SetWeight(ExpressionKey.BlinkRight, 0f);
            expression.SetWeight(ExpressionKey.Blink, 0f);
        }
    }
}
