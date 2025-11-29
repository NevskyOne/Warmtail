using UnityEngine;
using Zenject;
using System;
using Data;
using Data.Player;

public class DailySystem : MonoBehaviour
{
    [Inject] private GlobalData _globalData;
    public static Action OnDiscardedRecources = delegate {};
    public static Action OnLodedRecources = delegate {};

    void Awake()
    {
        CheckTime();
    }

    public void CheckTime()
    {
        string timeLastGameStr = _globalData.Get<RuntimePlayerData>().TimeLastGame;
        DateTime timeLastGame = DateTime.Parse(timeLastGameStr);
        DateTime timeNow = DateTime.UtcNow.AddHours(3);
        if (timeLastGame.Day != timeNow.Day || timeLastGame.Month != timeNow.Month || timeLastGame.Year != timeNow.Year)
            OnDiscardedRecources?.Invoke();
        else
            OnLodedRecources?.Invoke();
    }
}