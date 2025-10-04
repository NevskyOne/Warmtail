using System;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public string PlayerName;
    }
    
    [Serializable]
    public class RuntimePlayerData : IRuntimeData
    {
        public int Hp;
    }
}