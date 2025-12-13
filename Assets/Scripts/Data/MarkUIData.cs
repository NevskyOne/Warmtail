using UnityEngine;

namespace Data
{
    public record MarkUIData()
    {
        public GameObject Object { get; internal set; }
        public Vector2 WorldPos { get; internal set; }
    }
}