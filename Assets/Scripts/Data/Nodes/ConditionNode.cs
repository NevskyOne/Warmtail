using System;
using System.Collections.Generic;
using Data;
using Entities.UI;
using Systems.DataSystems;
using Systems.Dialogues;
using Systems.Dialogues.Nodes;
using UnityEngine;
using Zenject;

[NodeWidth(330)]
public class ConditionNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Output, SerializeField] private int _exit;
    public List<ConditionStruct> Conditions = new();

    [Inject] private GlobalDataSystem _globalData;
    [Inject] private DialogueSystem _dialogueSystem;
    
    public override void Activate()
    {
        var varDataList = _globalData.Get<DialogueVarData>().Variables;
        var targetOutput = 0;
        foreach (var condStruct in Conditions)
        {
            var dialogueVar = varDataList.Find(x => x.Name == condStruct.VarName);
            
            if((condStruct.Operation == ComparisonOperation.Equals && dialogueVar.Value == condStruct.Value) || 
               (condStruct.Operation == ComparisonOperation.NotEquals && dialogueVar.Value != condStruct.Value) ||
               (condStruct.Operation == ComparisonOperation.Less && float.Parse(dialogueVar.Value) < float.Parse(condStruct.Value)) ||
               (condStruct.Operation == ComparisonOperation.More && float.Parse(dialogueVar.Value) > float.Parse(condStruct.Value))) 
                break; 
            
            targetOutput++;
        }
        var dialogueGraph = (DialogueGraph)graph;
        dialogueGraph.Current = (BaseNode)GetOutputPort(targetOutput.ToString()).Connection.node;
        _dialogueSystem.IterateDialogue();
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
