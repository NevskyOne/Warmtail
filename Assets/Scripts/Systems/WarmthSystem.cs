using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Data.Player;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class WarmthSystem
    {
        private const int WarmthIncreaseRate = 1;
        private GlobalData _globalData;
        private WarmingState _state;
        private ResettableTimer _timer;
        
        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            _globalData.Edit<RuntimePlayerData>(data =>
            {
                data.CurrentWarmth = _globalData.Get<SavablePlayerData>().Stars * 10;
            });
            _globalData.SubscribeTo<SavablePlayerData>(IncreaseWarmth);
        }

        public void DecreaseWarmth(int value)
        {
            _globalData.Edit<RuntimePlayerData>(data =>
            {
                data.CurrentWarmth = Mathf.Max(data.CurrentWarmth - value, 0);
            });
            _state = WarmingState.Cooling;
            if (_timer != null)
                _timer.Start();
            else
                _timer = new ResettableTimer(3, IncreaseWarmth);
        }
        
        private async void IncreaseWarmth()
        {
            if (_state == WarmingState.Warming)
            {
                return;
            }
            
            _state = WarmingState.Warming;
            
            var max = _globalData.Get<SavablePlayerData>().Stars * 10;
            var current = _globalData.Get<RuntimePlayerData>().CurrentWarmth;
            while(current < max && _state == WarmingState.Warming){
                _globalData.Edit<RuntimePlayerData>(data =>
                {
                    current = Mathf.Min(current + WarmthIncreaseRate, max);
                    data.CurrentWarmth = current;
                });
                await UniTask.Delay(1000);
            }

            _state = WarmingState.None;
        }
    }

    public enum WarmingState
    {
        None, Cooling, Warming
    }
}