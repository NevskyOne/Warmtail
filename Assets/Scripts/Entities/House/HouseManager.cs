using UnityEngine;
using Zenject;
using Systems;

namespace Entities.House
{
    public class HouseManager : MonoBehaviour
    {
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
