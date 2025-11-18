using System;
using Data;
using Data.Player;
using Interfaces;
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
    
        [Inject]
        public void Construct(GlobalData globalData)
        {
            _globalData = globalData;
        }
        
        public void Warm()
        {
            _currentProgress -= 1;
            if (_currentProgress == 0)
            {
                Collect();
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
            Destroy(gameObject);
        }
    }
}