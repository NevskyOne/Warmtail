using Systems;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[NodeWidth(290)]
public class ActionNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Output, SerializeField] private int _exit;
    [SerializeField] private int _eventId;

    [Inject] private DialogueSystem _dialogueSystem;
    
    public override void Activate()
    {
        _dialogueSystem.Character.InvokeEvent(_eventId);
        
        _dialogueSystem.SetNewNode();
        _dialogueSystem.ActivateNewNode();
    }
}

