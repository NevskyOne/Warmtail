using Systems;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class UIStateMachine : MonoBehaviour
    {
        private CrossfadeSystem _crossfadeSystem;

        [Inject]
        private void Construct(CrossfadeSystem crossfade)
        {
            _crossfadeSystem = crossfade;
        }

        public async void SwitchCurrentMode()
        {
            await foreach (var (a, b) in _crossfadeSystem.CrossfadeTwins(1f))
            {
                //TODO: SomeLogic to switch ui
            }
        }
    }
}