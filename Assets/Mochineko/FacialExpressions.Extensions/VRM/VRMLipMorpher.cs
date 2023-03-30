#nullable enable
using System.Collections.Generic;
using Mochineko.FacialExpressions.LipSync;
using UniVRM10;

namespace Mochineko.FacialExpressions.Extensions.VRM
{
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
            else if (sample.viseme is Viseme.sil or Viseme.nn)
            {
                Reset();
            }
        }

        public void Reset()
        {
            MorphInto(new LipSample(Viseme.aa, 0f));
            MorphInto(new LipSample(Viseme.ih, 0f));
            MorphInto(new LipSample(Viseme.ou, 0f));
            MorphInto(new LipSample(Viseme.E, 0f));
            MorphInto(new LipSample(Viseme.oh, 0f));
        }
    }
}