using AYellowpaper.SerializedCollections;
using Data;
using UnityEngine;
using Zenject;
using Systems;
using Data.House;
using Entities.NPC;

namespace Entities.House
{
    public class HouseManager : MonoBehaviour
    {
        [SerializeField] private ItemsHouseIdConf _itemsHouseIdConf;
        [SerializeField] private SerializedDictionary<Characters,SpeakableCharacter> _npc;
        [HideInInspector] public HouseItemData[] IdsForHouseItemsData;
        [Inject] private PlacementSystem _placementSystem; 
        [Inject] private GlobalData _globalData; 

        void Awake()
        {
            IdsForHouseItemsData = _itemsHouseIdConf.IdsForHouseItemsData;
            EnableNpc(_globalData.Get<NpcSpawnData>().CurrentHomeNpc);
        }

        public void ApplyAllEditing()
        {
            _placementSystem.ApplyAllEditing();
        }
        public void CancelAll()
        {
            _placementSystem.CancelAll();
        }

        private void EnableNpc(Characters? character)
        {
            if(character != null)
                _npc[character.Value].ChangeState(true);
        }
    }
}
