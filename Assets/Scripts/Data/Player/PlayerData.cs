using System;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public string PlayerName;
        public int Stars;
        public int Shells;
    }
    
    [Serializable]
    public class RuntimePlayerData : IRuntimeData
    {
        public int Freeze;
        public int CurrentWarmth;
        public bool WasInGame;
    }
}
