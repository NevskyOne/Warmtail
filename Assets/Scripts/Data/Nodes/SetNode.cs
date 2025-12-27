using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Systems;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using Zenject;

namespace Data.Nodes
{
    public class SetNode : RuntimeNode
    {
        [SerializeField] private List<SetStruct> _variables = new();

        [Inject] private GlobalData _globalData;
        [Inject] private DialogueSystem _dialogueSystem;

        public override void Setup(INode node, Dictionary<INode, string> nodeIdMap)
        {
            var inputs = node.GetInputPorts().ToArray();
            var outputs = node.GetOutputPorts().ToArray();
            for (int i = 0; i < outputs.Length; i++)
            {
                _variables.Add(NodePortHelper.GetPortValue<SetStruct>(inputs[i+1]));
              
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
            foreach (var setStruct in _variables)
            {
                var varData = varDataList.Find(x => x.Name == setStruct.VarName);
                var index = varDataList.IndexOf(varData);
                switch (setStruct.Operation)
                {
                    case MathOperation.Assign:
                        varData.Value = setStruct.Value;
                        break;
                    case MathOperation.Add:
                        switch (varData.Type)
                        {
                            case DialogueVar.VarType.Int:
                                varData.Value = (int.Parse(varData.Value) + int.Parse(setStruct.Value)).ToString();
                                break;
                            case DialogueVar.VarType.Float:
                                varData.Value = (float.Parse(varData.Value) + float.Parse(setStruct.Value)).ToString(CultureInfo.InvariantCulture);
                                break;
                            case DialogueVar.VarType.String:
                                varData.Value += setStruct.Value;
                                break;
                        }
                        break;
                    case MathOperation.Subtract:
                        switch (varData.Type)
                        {
                            case DialogueVar.VarType.Int:
                                varData.Value = (int.Parse(varData.Value) - int.Parse(setStruct.Value)).ToString();
                                break;
                            case DialogueVar.VarType.Float:
                                varData.Value = (float.Parse(varData.Value) - float.Parse(setStruct.Value)).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                        break;
                    case MathOperation.Multiply:
                        switch (varData.Type)
                        {
                            case DialogueVar.VarType.Int:
                                varData.Value = (int.Parse(varData.Value) * int.Parse(setStruct.Value)).ToString();
                                break;
                            case DialogueVar.VarType.Float:
                                varData.Value = (float.Parse(varData.Value) * float.Parse(setStruct.Value)).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                        break;
                    case MathOperation.Divide:
                        switch (varData.Type)
                        {
                            case DialogueVar.VarType.Int:
                                varData.Value = (int.Parse(varData.Value) / int.Parse(setStruct.Value)).ToString();
                                break;
                            case DialogueVar.VarType.Float:
                                varData.Value = (float.Parse(varData.Value) / float.Parse(setStruct.Value)).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                        break;
                }

                _globalData.Edit<DialogueVarData>(x => x.Variables[index] = varData);
            }
        
            _dialogueSystem.SetNewNode();
            _dialogueSystem.ActivateNewNode();
        }
    }

    [Serializable]
    public class SetStruct
    {
        public string VarName;
        public MathOperation Operation;
        public string Value;
    }
    
    [Serializable]
    public enum MathOperation {Assign, Add, Subtract, Multiply, Divide}
}