using UnityEngine;
using UnityEngine.UI;
using Entities.House;

namespace Data.House
{
    [CreateAssetMenu(fileName = "House Item Data", menuName = "Configs/House Item Data")]
    public class HouseItemData : ScriptableObject
    {
        public int Id;
        public DraggableObject ItemPref;
        public Sprite Sprite;
        public int Price;
    }
}
