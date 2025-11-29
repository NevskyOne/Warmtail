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
        private int _warm;
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
            _warm -= 1;
            if (_warm == 0)
            {
                WarmExplosion();
            }
            else
            {
                if (_timer != null)
                    _timer.Start();
                else
                    _timer = new ResettableTimer(3, Reset);
            }
        }

        public void WarmExplosion()
        {
            _globalData.Edit<SavablePlayerData>((playerData) =>
            {
                playerData.Shells += _shellsAmount;
            });
            _globalData.Edit<ShellsData>(data => {
                data.ShellsActive[ConvertFloatsToString (transform.position.x, transform.position.y)] = false;
            });
            _timer.Stop();
            Destroy(gameObject);
        }
        
        public void Reset()
        {
            _warm = _warmCapacity;
        }

        private void LoadShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            if (!_globalData.Get<ShellsData>().ShellsActive[ConvertFloatsToString(x, y)])
                Destroy(gameObject);
        }

        private void DiscardShell()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            CheckShellData(x, y);
            _globalData.Edit<ShellsData>(data => data.ShellsActive[ConvertFloatsToString(x, y)] = true);
        }

        private void CheckShellData(float x, float y)
        {
            if (!_globalData.Get<ShellsData>().ShellsActive.ContainsKey(ConvertFloatsToString(x, y)))
                _globalData.Edit<ShellsData>(data => data.ShellsActive[ConvertFloatsToString(x, y)] = true);
        }

        private string ConvertFloatsToString(float x, float y)
        {
            return (x + " : " + y);
        }
    }
}