using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Data.Nodes
{
    [Serializable]
    public abstract class RuntimeNode
    {
        public string NodeId;
        [SerializeReference] public List<string> NextNodeIds = new();
        public abstract void Setup(INode node, Dictionary<INode, string> nodeIdMap);
        public abstract void Activate();
    }
}