using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Nodes;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Editor
{
    [ScriptedImporter(1, DialogueGraph.AssetExtension)]
    public class DialogueGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
            var runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();

            var nodeIdMap = new Dictionary<INode, string>();

            foreach (var node in editorGraph.GetNodes())
            {
                nodeIdMap[node] = Guid.NewGuid().ToString();
            }

            var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
            var entryPort = startNode?.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
            if(entryPort != null) runtimeGraph.EntryNodeId = nodeIdMap[entryPort.GetNode()];

            foreach (var iNode in editorGraph.GetNodes())
            {
                if (iNode is StartNode or EndNode) continue;
            }
        }

        private void ProcessNode(Node node, RuntimeNode runtimeNode, ref Dictionary<INode, string> nodeIdMap)
        {
            var fields = runtimeNode.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                //fields[i].SetValue(GetPortValue<>(node.GetInputPort(i)));
            }
        }

        private T GetPortValue<T>(IPort port)
        {
            if (port == null) return default;

            if (port.isConnected)
            {
                if (port.firstConnectedPort.GetNode() is IVariableNode variableNode)
                {
                    variableNode.variable.TryGetDefaultValue(out T value);
                    return value;
                }
            }

            port.TryGetValue(out T fallBackValue);
            return fallBackValue;
        }
    }
}