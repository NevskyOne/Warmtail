using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Player;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Entities.UI;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Entities.Probs
{
    public class Star : MonoBehaviour, IInteractable, IDeletable
    {
        private GlobalData _globalData;
        private MonologueVisuals _monologueVisuals;
        private List<int> _ids = new();
        
        [Inject]
        public void Construct(GlobalData globalData, MonologueVisuals monologueVisuals)
        {
            _globalData = globalData;
            _monologueVisuals = monologueVisuals;
            for (int i = 0; i < 30; i++)
            {
                _ids.Add(i);
            }
        }
    
        public void Interact()
        {
            if (_globalData == null) return;
            var newId = _ids.Except(_globalData.Get<SavablePlayerData>().SeenReplicas).GetRandom();
            _monologueVisuals.RequestSingleLine(newId);
            _globalData.Edit<SavablePlayerData>((playerData) =>
            {
                playerData.Stars += 1;
                playerData.SeenReplicas.Add(newId);
            });
            ((IDeletable)this).Delete(_globalData, gameObject.GetEntityId());
            Destroy(gameObject);
        }
    }
}
