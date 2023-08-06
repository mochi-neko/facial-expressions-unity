#nullable enable

namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Defines a morpher to change lip shape.
    /// </summary>
    public interface ILipMorpher
    {
        /// <summary>
        /// Morphs lip by specified viseme into specified weight.
        /// </summary>
        /// <param name="sample">Target lip sample.</param>
        void MorphInto(LipSample sample);
        
        /// <summary>
        /// Gets current weight of specified viseme.
        /// </summary>
        /// <param name="viseme">Target viseme.</param>
        /// <returns></returns>
        float GetWeightOf(Viseme viseme);
        
        /// <summary>
        /// Resets all morphing to default.
        /// </summary>
        void Reset();
    }
}