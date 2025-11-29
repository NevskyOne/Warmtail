using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public int Stars;
        public int Shells;
        public int ActiveLayers;
        public List<int> SeenReplicas;
        public SerializedDictionary<int, int> Inventory; // [id] = count
        public string TimeLastGame;
    }
    
    [Serializable]
    public class RuntimePlayerData : IRuntimeData
    {
        public int Freeze;
        public int CurrentWarmth;
        public bool WasInGame;
        public int Speed;
    }
}
