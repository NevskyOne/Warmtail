using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;

namespace Data.Nodes
{
    [Serializable]
    public abstract class RuntimeNode
    {
        public string NodeId;
        public readonly List<string> NextNodeIds = new();
        public abstract void Setup(INode node, Dictionary<INode, string> nodeIdMap);
        public abstract void Activate();
    }
}