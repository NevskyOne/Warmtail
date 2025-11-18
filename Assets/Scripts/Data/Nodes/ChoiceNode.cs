using System.Collections.Generic;
using Entities.UI;
using UnityEngine;
using Zenject;

[NodeWidth(330)]
public class ChoiceNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    public List<int> Choices = new();
    
    [Inject] private DialogueVisuals _dialogueVisuals;
    
    public override void Activate()
    {
        _dialogueVisuals.ShowOptions(Choices);
    }
}
