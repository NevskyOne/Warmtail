using System.Collections.Generic;
using Data.Nodes;
using UnityEngine;

namespace Data
{
    public class RuntimeDialogueGraph : ScriptableObject
    {
        public string EntryNodeId {get; set;}
        public List<RuntimeNode> AllNodes { get; set; } = new();
    }
}