using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System;
using Systems;

namespace Entities.House
{
    public class DraggableObjectUI : MonoBehaviour
    {
        [SerializeField] private DraggableObject _itemCopyingObjectPref;
        private DraggableObject _itemCopyingObject;
        private Vector2 _startClickPosition;
        
        [Inject] private PlacementSystem _placementSystem;

        public void PointerDownItem()
        {
            _itemCopyingObject = null;
            _startClickPosition = Mouse.current.position.ReadValue();
        }
        public void HoldItem()
        {
            if (Math.Abs(_startClickPosition.y - Mouse.current.position.ReadValue().y) < 50) 
            {
                if (_itemCopyingObject) Destroy(_itemCopyingObject.gameObject);
            }
            else
            {
                if (!_itemCopyingObject)
                {
                    _itemCopyingObject = _placementSystem.InstantiateDraggableObject(_itemCopyingObjectPref, false);
                }
                Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                _itemCopyingObject.transform.position = pos;
            }
        }
    }
}
