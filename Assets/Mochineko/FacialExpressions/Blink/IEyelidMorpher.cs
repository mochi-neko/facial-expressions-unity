#nullable enable
namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Defines a morpher of eyelid.
    /// </summary>
    public interface IEyelidMorpher
    {
        /// <summary>
        /// Morphs specified eyelid into specified weight. 
        /// </summary>
        /// <param name="sample"></param>
        void MorphInto(EyelidSample sample);

        /// <summary>
        /// Gets current weight of specified eyelid.
        /// </summary>
        /// <param name="eyelid"></param>
        /// <returns></returns>
        float GetWeightOf(Eyelid eyelid);

        /// <summary>
        /// Resets all morphing to default.
        /// </summary>
        void Reset();
    }
}