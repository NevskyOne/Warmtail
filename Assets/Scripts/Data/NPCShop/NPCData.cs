using AYellowpaper.SerializedCollections;

namespace Data.NPCShop
{
    public class NPCData : ISavableData
    {
        public SerializedDictionary<Characters, int> Levels;
        public SerializedDictionary<Characters, bool> BoughtLastItem;
    }
}