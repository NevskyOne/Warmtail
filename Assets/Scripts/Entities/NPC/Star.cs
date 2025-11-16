using Interfaces;
using UnityEngine;
using Zenject;
using Data;
using Data.Player;

public class Star : MonoBehaviour, IInteractable
{
    [SerializeField] private float maxHeatIncrease = 10f;
    
    private GlobalData _globalData;
    
    [Inject]
    public void Construct(GlobalData globalData)
    {
        this._globalData = globalData;
    }
    
    void IInteractable.Interact()
    {
        if (_globalData == null) return;
        
        _globalData.Edit<SavablePlayerData>((playerData) =>
        {
            playerData.MaxHeat += maxHeatIncrease;
        });
    }
}