using System;
using System.Collections.Generic;
using System.Globalization;
using Data;
using Entities.UI;
using Systems.DataSystems;
using Systems.Dialogues;
using Systems.Dialogues.Nodes;
using UnityEngine;
using Zenject;

[NodeWidth(330)]
public class SetNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Output, SerializeField] private int _exit;
    [SerializeField] private List<SetStruct> _variables;

    [Inject] private GlobalDataSystem _globalData;
    [Inject] private DialogueSystem _dialogueSystem;
    
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
        
        var dialogueGraph = (DialogueGraph)graph;
        dialogueGraph.Current = (BaseNode)GetOutputPort("_exit").Connection.node;
        _dialogueSystem.IterateDialogue();
    }
}

[Serializable]
public struct SetStruct
{
    public string VarName;
    public MathOperation Operation;
    public string Value;
}

public enum MathOperation {Assign, Add, Subtract, Multiply, Divide}
