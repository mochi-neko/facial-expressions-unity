#nullable enable
namespace Mochineko.FacialExpressions.Blink
{
    public interface IEyelidMorpher
    {
        void MorphInto(EyelidSample sample);
        void Reset();
    }
}