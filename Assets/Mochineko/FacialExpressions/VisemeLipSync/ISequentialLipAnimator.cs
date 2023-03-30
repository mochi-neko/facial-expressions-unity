#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.VisemeLipSync
{
    public interface ISequentialLipAnimator
    {
        UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken);
    }
}