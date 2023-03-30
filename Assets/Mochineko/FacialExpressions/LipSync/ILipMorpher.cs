#nullable enable

namespace Mochineko.FacialExpressions.LipSync
{
    public interface ILipMorpher
    {
        void MorphInto(LipSample sample);
        void Reset();
    }
}