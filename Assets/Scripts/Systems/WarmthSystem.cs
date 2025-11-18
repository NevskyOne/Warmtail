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
        private bool _isCooling;
        private CancellationTokenSource _token = new ();

        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            _globalData.SubscribeTo<SavablePlayerData>(IncreaseWarmth);
        }

        public async void DecreaseWarmth(int value)
        {
            _token.Cancel();
            _globalData.Edit<RuntimePlayerData>(data =>
            {
                data.CurrentWarmth = Mathf.Max(data.CurrentWarmth - value, 0);
            });
            _isCooling = true;
            await UniTask.Delay(3000, cancellationToken: _token.Token);
            _isCooling = false;
            IncreaseWarmth();
        }
        
        private async void IncreaseWarmth()
        {
            var max = _globalData.Get<SavablePlayerData>().Stars * 10;
            var current = _globalData.Get<RuntimePlayerData>().CurrentWarmth;
            while(current < max && !_isCooling){
                _globalData.Edit<RuntimePlayerData>(data =>
                {
                    current = Mathf.Min(current + WarmthIncreaseRate, max);
                    data.CurrentWarmth = Mathf.Min(data.CurrentWarmth + WarmthIncreaseRate, max);
                });
                await UniTask.Delay(1000);
            }
        }
    }
}