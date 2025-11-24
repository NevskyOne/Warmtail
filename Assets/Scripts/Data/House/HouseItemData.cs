using UnityEngine;
using Entities.House;

namespace Data.House
{
    [CreateAssetMenu(fileName = "New HouseItem Data", menuName = "HouseItem Data", order = 51)]
    public class HouseItemData : ScriptableObject
    {
        public int Id;
        public DraggableObject ItemPref;
        public int Price;
    }
}
