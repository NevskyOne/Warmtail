using UnityEngine;
using Zenject;
using Systems;
using Data.House;

namespace Entities.House
{
    public class HouseManager : MonoBehaviour
    {
        public HouseItemData[] IdsForHouseItemsData;
        [Inject] private PlacementSystem _placementSystem; 

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
