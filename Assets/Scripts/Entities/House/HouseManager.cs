using UnityEngine;
using Zenject;
using Systems;
using Data.House;

namespace Entities.House
{
    public class HouseManager : MonoBehaviour
    {
        [SerializeField] private ItemsHouseIdConf _itemsHouseIdConf;
        [HideInInspector] public HouseItemData[] IdsForHouseItemsData;
        [Inject] private PlacementSystem _placementSystem; 

        void Awake()
        {
            IdsForHouseItemsData = _itemsHouseIdConf.IdsForHouseItemsData;
        }

        public void ApplyAllEditing()
        {
            _placementSystem.ApplyAllEditing();
        }
        public void CancelAll()
        {
            _placementSystem.CancelAll();
        }
    }
}
