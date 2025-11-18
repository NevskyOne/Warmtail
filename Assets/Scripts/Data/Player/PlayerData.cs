using System;

namespace Data.Player
{
    [Serializable]
    public class SavablePlayerData : ISavableData {
        public string PlayerName;
        public float MaxHeat;
        public float CurrentHeat;
    }
    
    [Serializable]
    public class RuntimePlayerData : IRuntimeData
    {
        public int Hp;
    }
}
