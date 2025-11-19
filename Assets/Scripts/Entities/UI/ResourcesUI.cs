using System;
using System.Collections;
using Data;
using Data.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class ResourcesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _shells;
        [SerializeField] private TMP_Text _stars;
        private GlobalData _globalData;
        
        [Inject]
        public void Construct(GlobalData globalData)
        {
            _globalData = globalData;
            _globalData.SubscribeTo<SavablePlayerData>(UpdateUI);
        }
        
        private void Start() => UpdateUI();

        private void UpdateUI()
        {
            var data = _globalData.Get<SavablePlayerData>();
            _shells.text = data.Shells.ToString();
            _stars.text = data.Stars.ToString();
        }
    }
}