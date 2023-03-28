#nullable enable
using System.Collections.Generic;
using UniVRM10;

namespace Mochineko.FacialExpressions.VisemeLipSync.Extensions.VRM
{
    internal sealed class VRMLipMorpher : ILipMorpher
    {
        private static readonly IReadOnlyDictionary<Viseme, ExpressionKey> KeyMap
            = new Dictionary<Viseme, ExpressionKey>
            {
                [Viseme.aa] = ExpressionKey.Aa,
                [Viseme.ih] = ExpressionKey.Ih,
                [Viseme.ou] = ExpressionKey.Ou,
                [Viseme.E] = ExpressionKey.Ee,
                [Viseme.oh] = ExpressionKey.Ou,
            };

        private readonly Vrm10RuntimeExpression expression;

        public VRMLipMorpher(Vrm10RuntimeExpression expression)
        {
            this.expression = expression;
        }

        public void MorphInto(LipSample lipSample)
        {
            if (KeyMap.TryGetValue(lipSample.Viseme, out var key))
            {
                expression.SetWeight(key, lipSample.Weight);
            }
        }
    }
}