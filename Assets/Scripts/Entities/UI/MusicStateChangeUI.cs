using Entities.Sound;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class MusicStateChangeUI : MonoBehaviour
    {
        [Inject] private MusicStateSystem _musicStateSystem;

        public void Change(int state)
        {
            _musicStateSystem.ChangeMusicState(state);
        }
    }
}