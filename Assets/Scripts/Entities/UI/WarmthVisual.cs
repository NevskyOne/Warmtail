using Data;
using Data.Player;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
{
    public class WarmthVisual : MonoBehaviour
    {
        [Title("UI Elements")]
        [SerializeField, LabelText("Heat Fill Bar")] private Image _heatFillBar;

        private GlobalData _globalData;

        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            _globalData.SubscribeTo<RuntimePlayerData>(UpdateVisual);
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var data = _globalData.Get<SavablePlayerData>();
            var runtimeData = _globalData.Get<RuntimePlayerData>();
            
            if (data == null) return;

            if (_heatFillBar != null)
            {
                _heatFillBar.fillAmount = data.Stars > 0 
                    ? (float)runtimeData.CurrentWarmth / (data.Stars * 10)
                    : 0f;
            }
        }
    }
}
