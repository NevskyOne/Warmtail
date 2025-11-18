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
        [SerializeField, LabelText("Heat Text")] private TMP_Text _heatText;

        private GlobalData _globalData;

        [Inject]
        private void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            _globalData.SubscribeTo<SavablePlayerData>(UpdateVisual);
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var data = _globalData.Get<SavablePlayerData>();
            
            if (data == null) return;

            if (_heatFillBar != null)
            {
                _heatFillBar.fillAmount = data.MaxHeat > 0 
                    ? data.CurrentHeat / data.MaxHeat 
                    : 0f;
            }

            if (_heatText != null)
            {
                _heatText.text = $"{data.CurrentHeat:F1} / {data.MaxHeat:F1}";
            }
        }
    }
}
