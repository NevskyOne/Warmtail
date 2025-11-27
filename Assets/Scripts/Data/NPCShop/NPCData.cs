using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using Data.NPCShop;

namespace Data.NPCShop
{
    public class NPCData : ISavableData
    {
        public SerializedDictionary<Characters, int> Levels;
        public SerializedDictionary<Characters, bool> BoughtLastItem;
    }
}