using System;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public string PlayerName = "Bear";
        public int Level = 1;
        public int Coins;
    }
    
    [Serializable]
    public class RuntimePlayerData : IRuntimeData
    {
        public int Hp;
    }
}