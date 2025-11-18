using Interfaces;
using UnityEngine;
using Zenject;
using Data;
using Data.Player;

public class Star : MonoBehaviour, IInteractable
{
    private GlobalData _globalData;
    
    [Inject]
    public void Construct(GlobalData globalData)
    {
        _globalData = globalData;
    }
    
    public void Interact()
    {
        if (_globalData == null) return;
        
        _globalData.Edit<SavablePlayerData>((playerData) =>
        {
            playerData.Stars += 1;
        });
        Destroy(gameObject);
    }
}
