#nullable enable
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.FacialExpressions.Blink
{
    public interface IEyelidAnimator
    {
        UniTask AnimateAsync(
            IEnumerable<EyelidAnimationFrame> frames,
            bool loop,
            CancellationToken cancellationToken);
    }
}