using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    [Serializable]
    public class NpcSpawnData : ISavableData
    {
        public SerializedDictionary<int, List<int>> NpcSpawnerData; //[character] = {idPrefab, idPosition}
    }
}
