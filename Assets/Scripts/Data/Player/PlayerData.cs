using System;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public int Stars;
        public int Shells;
        public int ActiveLayers; 
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
