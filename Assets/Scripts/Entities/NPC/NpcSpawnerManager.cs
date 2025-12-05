using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Rng = UnityEngine.Random;
using UnityEngine;
using Zenject;
using Systems;
using Data;

namespace Entities.NPC
{
    public class NpcSpawnerManager : MonoBehaviour
    {
        public SerializedDictionary<Characters, List<GameObject>> NpcSpawner;
        [Inject] private GlobalData _globalData;
        [SerializeField] private Transform[] _parentNpc;

        private void Awake()
        {
            Daily.OnLodedRecources += LoadNpc;
            Daily.OnDiscardedRecources += DiscardNpc;
        }

        private void OnDestroy()
        {
            Daily.OnLodedRecources -= LoadNpc;
            Daily.OnDiscardedRecources -= DiscardNpc;
        }

        private void LoadNpc()
        {
            foreach (var auto in _globalData.Get<NpcSpawnData>().NpcSpawnerData)
            {
                NpcSpawner[(Characters)auto.Key][auto.Value].SetActive(true);
            }
        }
        private void DiscardNpc()
        {
            _globalData.Edit<NpcSpawnData>(data =>
            {
                data.NpcSpawnerData = new();
                for (int i = 0; i < NpcSpawner.Count; i ++)
                {
                    int prefId = Rng.Range(0, NpcSpawner.Keys.Count);
                    int posId = Rng.Range(0, NpcSpawner[(Characters)prefId].Count);
                    data.NpcSpawnerData[prefId] = posId;
                }
            });
            LoadNpc();
        }
    }
}
