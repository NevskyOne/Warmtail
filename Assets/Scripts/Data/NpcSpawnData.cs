using AYellowpaper.SerializedCollections;
using System;

namespace Data
{
    [Serializable]
    public class NpcSpawnData : ISavableData
    {
        public SerializedDictionary<int, int> NpcSpawnerData; //[character] = [idPrefab]
        public Characters CurrentHomeNpc;
    }
}
