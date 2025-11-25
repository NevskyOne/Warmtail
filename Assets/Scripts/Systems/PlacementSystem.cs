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
        private HouseManager _houseManager;
        public int CountItemOnTheScene;

        [Inject] private DiContainer _diContainer;
        [Inject]
        private void Construct(GlobalData globalData, HouseManager houseManager)
        {
            _houseManager = houseManager;
            _globalData = globalData;
            
            var houseItems = _globalData.Get<HouseData>().PlacedHouseItems;
            foreach (PairForHouseItem item in houseItems)
            {
                DraggableObject gm = InstantiateDraggableObject(_houseManager.IdsForHouseItemsData[item.HouseItemDataId].ItemPref, new Vector2(item.PositionX, item.PositionY), true);
                gm.transform.position = new Vector2(item.PositionX, item.PositionY);
            }
        }
        public DraggableObject InstantiateDraggableObject(DraggableObject draggableObject, Vector2 pos, bool isItemConfirmed)
        {
            DraggableObject obj = _diContainer.InstantiatePrefab(draggableObject).GetComponent<DraggableObject>();
            if (pos.x != Vector2.positiveInfinity.x) obj.transform.position = pos;
            obj.Initialize(isItemConfirmed);
            return obj;
        }

        public void AddEditingItem(int idInArray, Vector2 currentPos)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                houseData.PlacedHouseItems.Add(new (idInArray, currentPos.x, currentPos.y));
            });
        }
        public void ReplaceEditingItem(int idInArray, Vector2 posOnConfirmedState, Vector2 currentPos)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                for (int ind = 0; ind < houseData.PlacedHouseItems.Count; ind++) {
                    if (houseData.PlacedHouseItems[ind].HouseItemDataId == idInArray && houseData.PlacedHouseItems[ind].PositionX == posOnConfirmedState.x && houseData.PlacedHouseItems[ind].PositionY == posOnConfirmedState.y) {
                        houseData.PlacedHouseItems[ind] = new PairForHouseItem(idInArray, currentPos.x, currentPos.y);
                        break;
                    }
                }
            });
        }
        public void RemoveEditingItem(int idInArray, Vector2 posOnConfirmedState)
        {
            _globalData.Edit<HouseData>(houseData =>
            {
                for (int ind = 0; ind < houseData.PlacedHouseItems.Count; ind++) {
                    if (houseData.PlacedHouseItems[ind].HouseItemDataId == idInArray && houseData.PlacedHouseItems[ind].PositionX == posOnConfirmedState.x && houseData.PlacedHouseItems[ind].PositionY == posOnConfirmedState.y) {
                        houseData.PlacedHouseItems.RemoveAt(ind);
                        break;
                    }
                }
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