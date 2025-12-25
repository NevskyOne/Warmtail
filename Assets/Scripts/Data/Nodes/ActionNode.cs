using System.Collections.Generic;
using Systems;
using Unity.GraphToolkit.Editor;
using Zenject;
using UnityEngine;

namespace Data.Nodes
{
    public class ActionNode : RuntimeNode
    {
        [field: SerializeField] public int EventInd { get; private set; }

        [Inject] private DialogueSystem _dialogueSystem;

        public override void Setup(INode node, Dictionary<INode, string> nodeIdMap)
        {
            var nextNode = node.GetOutputPortByName("out")?.firstConnectedPort;
            if(nextNode != null)
                NextNodeIds.Add(nodeIdMap[nextNode.GetNode()]);
            EventInd = NodePortHelper.GetPortValue<int>(node.GetInputPortByName("Event Index"));
        }

        public override void Activate()
        {
            _dialogueSystem.Character.InvokeEvent(EventInd);
        
            _dialogueSystem.SetNewNode();
            _dialogueSystem.ActivateNewNode();
        }
    }
}

