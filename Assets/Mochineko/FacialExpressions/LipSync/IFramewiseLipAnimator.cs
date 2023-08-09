#nullable enable
namespace Mochineko.FacialExpressions.LipSync
{
    /// <summary>
    /// Defines an animator to update lip animation per game engine frame.
    /// </summary>
    public interface IFramewiseLipAnimator
    {
        /// <summary>
        /// Updates animation.
        /// </summary>
        void Update();

        /// <summary>
        /// Resets animation to default.
        /// </summary>
        void Reset();
    }
}
