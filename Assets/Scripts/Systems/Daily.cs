using UnityEngine;
using Zenject;
using System;
using Data;
using Data.Player;

namespace Systems
{
    public class Daily : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        public static Action OnDiscardedRecources = delegate {};
        public static Action OnLodedRecources = delegate {};

        void Start()
        {
            CheckTime();
        }

        public void CheckTime()
        {
            string timeLastGameStr = _globalData.Get<SavablePlayerData>().TimeLastGame;
            DateTime timeLastGame = new (2000, 1, 1, 0, 0, 0, 0);
            if (!string.IsNullOrEmpty(timeLastGameStr)) timeLastGame = DateTime.Parse(timeLastGameStr);

            DateTime timeNow = DateTime.UtcNow.AddHours(3);
            _globalData.Edit<SavablePlayerData>(data => data.TimeLastGame = timeNow.ToString());
            if (timeLastGame.Day != timeNow.Day || timeLastGame.Month != timeNow.Month || timeLastGame.Year != timeNow.Year)
                OnDiscardedRecources?.Invoke();
            else
                OnLodedRecources?.Invoke();
        }
    }
}