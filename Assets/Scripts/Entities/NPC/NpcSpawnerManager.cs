using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Rng = UnityEngine.Random;
using UnityEngine;
using System.Linq;
using Zenject;
using Systems;
using System;
using Data;

namespace Entities.NPC
{
    public class NpcSpawnerManager : MonoBehaviour
    {
        public SerializedDictionary<List<GameObject>, List<Vector3>> NpcSpawner;
        [Inject] private GlobalData _globalData;
        [SerializeField] private Transform[] parentNpc;

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
                Vector3 pos = NpcSpawner.ElementAt(auto.Key).Value[auto.Value[1]];
                int posZ = (int)Math.Round(pos.z);
                pos.z = 0;
                Instantiate(NpcSpawner.ElementAt(auto.Key).Key[auto.Value[0]], pos, Quaternion.identity, parentNpc[posZ]);
            }
        }
        private void DiscardNpc()
        {
            _globalData.Edit<NpcSpawnData>(data =>
            {
                data.NpcSpawnerData = new();
                for (int i = 0; i < NpcSpawner.Count; i ++)
                {
                    int prefId = Rng.Range(0, NpcSpawner.ElementAt(i).Key.Count);
                    int posId = Rng.Range(0, NpcSpawner.ElementAt(i).Value.Count);
                    Vector3 pos = NpcSpawner.ElementAt(i).Value[posId];
                    int posZ = (int)Math.Round(pos.z);
                    pos.z = 0;

                    data.NpcSpawnerData[i] = new(){prefId, posId};
                    Instantiate(NpcSpawner.ElementAt(i).Key[prefId], pos, Quaternion.identity, parentNpc[posZ]);
                }
            });
        }
    }
}
