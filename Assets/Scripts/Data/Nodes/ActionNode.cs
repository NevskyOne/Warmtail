using Systems;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[NodeWidth(290)]
public class ActionNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Output, SerializeField] private int _exit;
    [SerializeField] private UnityEvent _event;

    [Inject] private DialogueSystem _dialogueSystem;
    
    public override void Activate()
    {
        _event.Invoke();
        
        _dialogueSystem.SetNewNode();
        _dialogueSystem.ActivateNewNode();
    }
}

