using System.Collections.Generic;

namespace Data
{
    public class WorldData : ISavableData
    {
        public readonly Dictionary<int, bool> SavableObjects = new();
    }
}