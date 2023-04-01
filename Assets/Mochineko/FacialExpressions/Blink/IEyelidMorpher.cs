#nullable enable
namespace Mochineko.FacialExpressions.Blink
{
    public interface IEyelidMorpher
    {
        void MorphInto(EyelidSample sample);
        float GetWeightOf(Eyelid eyelid);
        void Reset();
    }
}