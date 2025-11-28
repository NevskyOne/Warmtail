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
            _timer.Stop();
            Destroy(gameObject);
        }

        public void Reset()
        {
            _warm = _warmCapacity;
        }
    }
}