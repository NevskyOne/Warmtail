using AYellowpaper.SerializedCollections;

namespace Data
{
    public class NPCData : ISavableData
    {
        public SerializedDictionary<Characters, int> Levels;
    }
}