using System.Collections.Generic;
using Entities.UI;
using Systems.Dialogues;
using UnityEngine;
using Zenject;

[NodeWidth(330)]
public class ChoiceNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    public List<string> Choices = new();
    
    [Inject] private DialogueSystem _dialogueSystem;
    
    public override void Activate()
    {
        _dialogueSystem.ShowOptions(Choices);
    }
}
