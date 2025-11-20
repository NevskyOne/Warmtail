using Data;
using Data.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

namespace Entities.Core
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private TimelineAsset _longTimeline;
        [SerializeField] private TimelineAsset _shortTimeline;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
        }

        [Inject]
        private void Construct(GlobalData data)
        {
            _director.Play(data.Get<RuntimePlayerData>().WasInGame ? _shortTimeline : _longTimeline);
            data.Edit<RuntimePlayerData>(playerData => playerData.WasInGame = true);
        }
    }
}