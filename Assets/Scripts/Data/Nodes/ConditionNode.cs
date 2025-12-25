using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Unity.GraphToolkit.Editor;
using Zenject;
using UnityEngine;

namespace Data.Nodes
{
    public class ConditionNode : RuntimeNode
    {
        [field: SerializeField] public List<ConditionStruct> _conditions{ get; private set; } = new();

        [Inject] private GlobalData _globalData;
        [Inject] private DialogueSystem _dialogueSystem;

        public override void Setup(INode node, Dictionary<INode, string> nodeIdMap)
        {
            var inputs = node.GetInputPorts().ToArray();
            var outputs = node.GetOutputPorts().ToArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                _conditions.Add(NodePortHelper.GetPortValue<ConditionStruct>(inputs[i]));
              
                var nextNode = outputs[i]?.firstConnectedPort;
                if (nextNode != null)
                {
                    NextNodeIds.Add(nodeIdMap[nextNode.GetNode()]);
                }
            }
        }

        public override void Activate()
        {
            var varDataList = _globalData.Get<DialogueVarData>().Variables;
            var targetOutput = 0;
            foreach (var condStruct in _conditions)
            {
                var dialogueVar = varDataList.Find(x => x.Name == condStruct.VarName);
            
                if((condStruct.Operation == ComparisonOperation.Equals && dialogueVar.Value == condStruct.Value) || 
                   (condStruct.Operation == ComparisonOperation.NotEquals && dialogueVar.Value != condStruct.Value) ||
                   (condStruct.Operation == ComparisonOperation.Less && float.Parse(dialogueVar.Value) < float.Parse(condStruct.Value)) ||
                   (condStruct.Operation == ComparisonOperation.More && float.Parse(dialogueVar.Value) > float.Parse(condStruct.Value))) 
                    break; 
            
                targetOutput++;
            }
        
            _dialogueSystem.SetNewNode(targetOutput);
            _dialogueSystem.ActivateNewNode();
        }
    }

    [Serializable]
    public struct ConditionStruct
    {
        public string VarName;
        public ComparisonOperation Operation;
        public string Value;
    }

    public enum ComparisonOperation {Equals, NotEquals, Less, More}
}