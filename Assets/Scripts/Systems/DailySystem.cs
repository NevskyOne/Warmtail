using UnityEngine;
using System;

public class DailySystem : MonoBehaviour
{
    [SerializeField] private DateTime utcNow;

    void Start()
    {
        SetTime();
    }
    public void SetTime()
    {
        utcNow = DateTime.UtcNow;
        Debug.Log(utcNow);
    }
}
