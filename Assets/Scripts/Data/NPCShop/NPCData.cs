using AYellowpaper.SerializedCollections;

namespace Data.NPCShop
{
    public class NPCData : ISavableData
    {
        public SerializedDictionary<Character, int> Levels;
        public SerializedDictionary<Character, bool> BoughtLastItem;
    }
}