using UnityEngine;
using Zenject;
using System;
using Data;
using Data.Player;

public class DailySystem : MonoBehaviour
{
    [Inject] private GlobalData _globalData;

    void Start()
    {
        SetTime();
    }
    private void ResetResources()
    {
        Debug.Log("reset daily");
    }
    public void SetTime()
    {
        string timeLastGameStr = _globalData.Get<RuntimePlayerData>().TimeLastGame;
        DateTime timeLastGame = DateTime.Parse(timeLastGameStr);
        DateTime timeNow = DateTime.UtcNow.AddHours(3);
        Debug.Log("saved = " + timeLastGameStr + ";;;;  now = " + timeNow);
        if (timeLastGame.Day != timeNow.Day || timeLastGame.Month != timeNow.Month || timeLastGame.Year != timeNow.Year)
            ResetResources();
    }
}
//28.11.2025 20:00:00        03:59:47.5953888      0