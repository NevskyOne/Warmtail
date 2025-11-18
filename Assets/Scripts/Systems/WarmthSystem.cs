using Data;
using Data.Player;
using UnityEngine;
using Zenject;

namespace Systems
{
    
    public class WarmthSystem : ITickable
    {
        private const float HeatIncreaseRate = 1f;
        
        private GlobalData _globalData;

        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
        }

        public void Tick()
        {
            UpdateHeat();
        }

        private void UpdateHeat()
        {
            _globalData.Edit<SavablePlayerData>(data =>
            {
                if (data == null) return;
                
                data.CurrentHeat += HeatIncreaseRate * Time.deltaTime;
                data.CurrentHeat = Mathf.Min(data.CurrentHeat, data.MaxHeat);
            });
        }
    }
}