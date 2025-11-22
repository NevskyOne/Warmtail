using System.Collections.Generic;
using UnityEngine;

// namespace Data
// {
    public class HouseData : ISavableData
    {
        public List<HouseItemData> ObjectsInfo = new();
        public HouseData ()
        {
            //ObjectsId[new HousesItem(0, )]
        }
    }
// }