using Systems;
using UnityEngine;
using Zenject;

[NodeWidth(100)]
[NodeTint("#8c3760")]
public class EndNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Inject] private DialogueSystem _dialogueSystem;

    public override void Activate()
    {
        _dialogueSystem.EndDialogue();
    }
}