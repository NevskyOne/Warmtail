using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using Zenject;
using Data;
using Data.House;
using Entities.House;

namespace Systems
{
    public class PlacementSystem
    {
        private List<PairForHouseItem> _houseItemsEditingInfo = new();
        public static Action OnApplyedAll = delegate {};
        public static Action OnCanceledAll = delegate {};
        private GlobalData _globalData;
        public int CountItemOnTheScene;

        [Inject] private DiContainer _diContainer;
        [Inject]
        private void Construct(GlobalData globalData)
        {            
            _globalData = globalData;
            
            var houseItems = _globalData.Get<HouseData>().PlacedHouseItems;
            foreach (PairForHouseItem item in houseItems)
            {
                DraggableObject gm = InstantiateDraggableObject(item.HouseItemData.ItemPref, true);
                gm.transform.position = item.Position;
            }
        }
        public DraggableObject InstantiateDraggableObject(DraggableObject draggableObject, bool isItemConfirmed)
        {
            DraggableObject obj = _diContainer.InstantiatePrefab(draggableObject).GetComponent<DraggableObject>();
            obj.Initialize(isItemConfirmed);
            return obj;
        }

        public void AddEditingItem(HouseItemData data, Vector2 currentPos)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                houseData.PlacedHouseItems.Add(new (data, currentPos));
            });
        }
        public void ReplaceEditingItem(HouseItemData data, Vector2 posOnConfirmedState, Vector2 currentPos)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                int ind = houseData.PlacedHouseItems.IndexOf(new PairForHouseItem(data, posOnConfirmedState));
                houseData.PlacedHouseItems[ind] = new PairForHouseItem(data, currentPos);
            });
        }
        public void RemoveEditingItem(HouseItemData data, Vector2 posOnConfirmedState)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                houseData.PlacedHouseItems.Remove(new PairForHouseItem(data, posOnConfirmedState));
            });
        }
        public void ApplyAllEditing()
        {
            OnApplyedAll?.Invoke();
        }
        public void CancelAll()
        {
            OnCanceledAll?.Invoke();
        }
    }
}