#nullable enable

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    public interface ILipMorpher
    {
        void MorphInto(LipSample sample);
        void Reset();
    }
}