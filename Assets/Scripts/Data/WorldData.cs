using System.Collections.Generic;
using Interfaces;

namespace Data
{
    public class WorldData : ISavableData
    {
        public List<int> DeletedObjects = new();
    }
}