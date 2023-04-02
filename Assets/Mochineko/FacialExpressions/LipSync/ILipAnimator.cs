#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.LipSync
{
    public interface ILipAnimator
    {
        UniTask AnimateAsync(
            IEnumerable<LipAnimationFrame> frames,
            CancellationToken cancellationToken);
    }
}