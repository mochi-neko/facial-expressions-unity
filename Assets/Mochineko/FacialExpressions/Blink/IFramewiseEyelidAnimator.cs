#nullable enable
namespace Mochineko.FacialExpressions.Blink
{
    /// <summary>
    /// Defines an animator to update eyelid animation per game engine frame.
    /// </summary>
    public interface IFramewiseEyelidAnimator
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
