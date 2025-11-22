using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Zenject;

// namespace Systems
// {
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private DraggableObject _itemObjectPref;
        private List<KeyValuePair<HouseItemData, Vector2>> _houseItemsInfo = new();
        private List<KeyValuePair<HouseItemData, Vector2>> _houseItemsEditingInfo = new();
        public static Action OnApplyedAll = delegate {};
        public static Action OnCanceledAll = delegate {};
        private HouseData _houseData;
        public int CountItemOnTheScene;

        [Inject]
        private void Construct(HouseData houseData)
        {
            _houseData = houseData;
        }
        public void AddEditingItem(HouseItemData houseItemData, Vector2 pos)
        {
            _houseItemsEditingInfo.Add(new KeyValuePair<HouseItemData, Vector2>(houseItemData, pos));
        }
        void Awake()
        {
            _inputActions.FindActionMap("House").Enable();
            foreach (var obj in _houseData.ObjectsInfo)
            {
                // DraggableObject gm = Instantiate(_itemObjectPref, obj.Position, Quaternion.identity);
                // gm.Initialize(obj.Sprite, obj.Scale, false);
            }
        }
        void OnDestroy()
        {
            _inputActions.FindActionMap("House").Disable();
        }
        public void ApplyAll()
        {
            OnApplyedAll?.Invoke();
        }
        public void CancelAll()
        {
            foreach (var item in _houseItemsEditingInfo)
            {
                _houseItemsInfo.Add(item);
            }
            _houseItemsEditingInfo.Clear();
            OnCanceledAll?.Invoke();
        }
    }
//}