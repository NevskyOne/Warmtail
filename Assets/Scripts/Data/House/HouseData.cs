
using System.Collections.Generic;
using System;
using TriInspector;
using UnityEngine;

namespace Data.House
{
    [Serializable]
    public class HouseData : ISavableData
    {
        [SerializeField, TableList(Draggable = true, AlwaysExpanded = true)]
        public List<PairForHouseItem> PlacedHouseItems = new();
    }
    [Serializable]
    public class PairForHouseItem
    {
        public HouseItemData HouseItemData;
        public Vector2 Position;
        public PairForHouseItem(HouseItemData houseItemData, Vector2 position)
        {
            HouseItemData = houseItemData;
            Position = position;
        }
    }
}