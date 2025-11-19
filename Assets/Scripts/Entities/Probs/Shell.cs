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
    }
}