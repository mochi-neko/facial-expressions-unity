#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    public interface ILipAnimator
    {
        UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> lipAnimationFrames,
            CancellationToken cancellationToken);
    }
}