using Systems.Dialogues;
using Systems.Dialogues.Nodes;
using UnityEngine;
using Zenject;

[NodeWidth(100)]
[NodeTint("#378c59")]
public class StartNode : BaseNode
{
    [Output, SerializeField] private int _exit;
    [Inject] private DialogueSystem _dialogueSystem;
    
    public override void Activate()
    {
        var dialogueGraph = (DialogueGraph)graph;
        dialogueGraph.Current = (BaseNode)dialogueGraph.Current.GetOutputPort("_exit").Connection.node;
        _dialogueSystem.StartDialogue((DialogueGraph)graph);
    }
}

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
