using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Systems
{
    public class CrossfadeSystem
    {
        //returns 
        public async IAsyncEnumerable<(float, float value)> CrossfadeTwins(float duration)
        {
            var value = 0f;
            var tween = PrimeTween.Tween.Custom(0f, 1f, duration, x => value = x);
            
            while (tween.isAlive)
            {
                yield return (1f - value, value);
                await UniTask.Yield();
            }
            
            yield return (0f, 1f);
        }
    }
}