using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Data;
using TriInspector;
using UnityEngine;

namespace Entities.UI
{
    public class PopupSystem : MonoBehaviour
    {
        [SerializeField, TableList(Draggable = false)] private List<PopupBase> _normalPopupObjects;
        [SerializeField, SerializedDictionary] private SerializedDictionary<PopupType, Transform> _popupHolders;
        
        public void ShowPopup(PopupBase data)
        {
            var normalData = _normalPopupObjects.Find(x => x.Type == data.Type);
            data.Setup(normalData, _popupHolders[data.Type]);
            
        }
    }
}