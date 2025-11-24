using System;
using System.Collections.Generic;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public int Stars;
        public int Shells;
        public int ActiveLayers;
        public List<int> SeenReplicas;
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
