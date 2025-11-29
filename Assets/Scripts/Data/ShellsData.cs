using AYellowpaper.SerializedCollections;
using System;

namespace Data
{
    [Serializable]
    public class ShellsData : ISavableData
    {
        public SerializedDictionary<Position, bool> ShellsActive;
    }

    [Serializable]
    public class Position
    {
        public float X;
        public float Y;
        public Position (float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}