using System;
using Data;
using Data.Player;
using Interfaces;
using Systems;
using UnityEngine;
using Zenject;

namespace Entities.Probs
{
    public class Shell : MonoBehaviour, IWarmable
    {
        [SerializeField] private int _warmCapacity;
        [SerializeField] private int _shellsAmount;
        private GlobalData _globalData;
        private int _currentProgress;
        private ResettableTimer _timer;
    
        [Inject]
        public void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            Reset();
            DailySystem.OnLodedRecources += LoadShell;
            DailySystem.OnDiscardedRecources += DiscardShell;
        }

        private void OnDestroy()
        {
            DailySystem.OnLodedRecources -= LoadShell;
            DailySystem.OnDiscardedRecources -= DiscardShell;
        }
        
        public void Warm()
        {
            _currentProgress -= 1;
            if (_currentProgress == 0)
            {
                Collect();
            }
            else
            {
                if (_timer != null)
                    _timer.Reset(3);
                else
                    _timer = new ResettableTimer(3, Reset);
            }
        }

        public void WarmExplosion()
        {
            Collect();
        }

        public void Reset()
        {
            _currentProgress = _warmCapacity;
        }

        private void Collect()
        {
            _globalData.Edit<SavablePlayerData>((playerData) =>
            {
                playerData.Shells += _shellsAmount;
            });
            _timer.Stop();
            Destroy(gameObject);
        }

        private void LoadShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            if (!_globalData.Get<ShellsData>().ShellsActive[new (x, y)])
                Destroy(gameObject);
        }

        private void DiscardShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            _globalData.Edit<ShellsData>(data => data.ShellsActive[new (x, y)] = true);
        }

        private void CheckShellData(float x, float y)
        {
            if (!_globalData.Get<ShellsData>().ShellsActive.ContainsKey(new (x, y)))
                _globalData.Edit<ShellsData>(data => data.ShellsActive[new (x, y)] = true);
        }
    }
}