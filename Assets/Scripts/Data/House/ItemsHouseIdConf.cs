using UnityEngine;

namespace Data.House
{
    [CreateAssetMenu(fileName = "Items House Id Data", menuName = "Configs/Items House Ids Data")]
    public class ItemsHouseIdConf : ScriptableObject
    {
        public HouseItemData[] IdsForHouseItemsData;
    }
}
