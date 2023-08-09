#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.LipSync;
using UniVRM10;

namespace Mochineko.FacialExpressions.Extensions.VRM
{
    /// <summary>
    /// A lip morpher for VRM models.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class VRMLipMorpher : ILipMorpher
    {
        private readonly Vrm10RuntimeExpression expression;

        private static readonly IReadOnlyDictionary<Viseme, ExpressionKey> KeyMap
            = new Dictionary<Viseme, ExpressionKey>
            {
                [Viseme.aa] = ExpressionKey.Aa,
                [Viseme.ih] = ExpressionKey.Ih,
                [Viseme.ou] = ExpressionKey.Ou,
                [Viseme.E] = ExpressionKey.Ee,
                [Viseme.oh] = ExpressionKey.Ou,
            };

        /// <summary>
        /// Create a lip morpher for VRM models.
        /// </summary>
        /// <param name="expression">Target expression of VRM instance.</param>
        public VRMLipMorpher(Vrm10RuntimeExpression expression)
        {
            this.expression = expression;
        }

        public void MorphInto(LipSample sample)
        {
            if (KeyMap.TryGetValue(sample.viseme, out var key))
            {
                expression.SetWeight(key, sample.weight);
            }
        }

        public float GetWeightOf(Viseme viseme)
        {
            if (KeyMap.TryGetValue(viseme, out var key))
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
            expression.SetWeight(ExpressionKey.Aa, 0f);
            expression.SetWeight(ExpressionKey.Ih, 0f);
            expression.SetWeight(ExpressionKey.Ou, 0f);
            expression.SetWeight(ExpressionKey.Ee, 0f);
            expression.SetWeight(ExpressionKey.Oh, 0f);
        }
    }
}
