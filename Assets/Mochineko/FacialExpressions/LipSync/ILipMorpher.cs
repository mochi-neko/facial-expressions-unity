#nullable enable

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Defines an object that morphs lip.
    /// </summary>
    public interface ILipMorpher
    {
        /// <summary>
        /// Morphs specified viseme into specified weight.
        /// </summary>
        /// <param name="sample"></param>
        void MorphInto(LipSample sample);
        /// <summary>
        /// Gets current weight of specified viseme.
        /// </summary>
        /// <param name="viseme"></param>
        /// <returns></returns>
        float GetWeightOf(Viseme viseme);
        /// <summary>
        /// Resets all morphing to default.
        /// </summary>
        void Reset();
    }
}