using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.House
{
    public class DraggableObjectUI : MonoBehaviour
    {
        [SerializeField] private DraggableObject _itemCopyingObjectPref;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Vector2 _itemCopyingScale = new Vector2(1, 1);
        private DraggableObject _itemCopyingObject;
        private Vector2 _startClickPosition;
        
        public void PointerDownItem()
        {
            _itemCopyingObject = null;
            _startClickPosition = Mouse.current.position.ReadValue();
        }
        public void PointerUpItem()
        {
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
                    _itemCopyingObject = Instantiate(_itemCopyingObjectPref);
                    _itemCopyingObject.Initialize(_sprite, _itemCopyingScale);
                }
                Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                _itemCopyingObject.transform.position = pos;
            }
        }
    }
}
