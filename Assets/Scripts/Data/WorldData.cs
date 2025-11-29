using System.Collections.Generic;

namespace Data
{
    public class WorldData : ISavableData
    {
        public readonly Dictionary<string, bool> SavableObjects = new();
    }
}