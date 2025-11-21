using System;
using Cysharp.Threading.Tasks;
using Entities.PlayerScripts;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Entities.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private bool _destroyAfter;
        [SerializeField] private float _stunTime;
        [SerializeField] private Transform _target;
        [SerializeField] private float _zoom;
        private Player _player;
        private CinemachineCamera _camera;
        private float _lastZoom;
        
        [Inject]
        private void Construct(Player player, CinemachineCamera cam)
        {
            _camera = cam;
            _player = player;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player.DisableAllAbilities();
                _camera.Target.TrackingTarget = _target;
                _lastZoom = _camera.Lens.OrthographicSize;
                _camera.Lens.Lerp(new LensSettings
                {
                    OrthographicSize = _zoom,
                    FarClipPlane = _camera.Lens.FarClipPlane,
                    NearClipPlane = _camera.Lens.NearClipPlane,
                },500);
                Disable();
            }
        }

        private async void Disable()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_stunTime));
            if (_destroyAfter)
            {
                Destroy(this);
            }
            _player.EnableLastAbilities();
            _camera.Target.TrackingTarget = _player.transform;
            _camera.Lens.Lerp(new LensSettings
            {
                OrthographicSize = _lastZoom,
                FarClipPlane = _camera.Lens.FarClipPlane,
                NearClipPlane = _camera.Lens.NearClipPlane,
            }, 500);
        }
    }
}