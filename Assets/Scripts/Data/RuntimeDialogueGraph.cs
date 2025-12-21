using System.Collections.Generic;
using Data.Nodes;
using UnityEngine;

namespace Data
{
    public class RuntimeDialogueGraph : ScriptableObject
    {
        [field: SerializeField] public string EntryNodeId {get; set;}
        public List<RuntimeNode> AllNodes { get; set; } = new();
        [field: SerializeField] public int DialogueId { get; set; }
    }
}