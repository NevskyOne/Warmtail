using UnityEngine;
using Zenject;

namespace Systems.Dialogues.Nodes
{
    [NodeWidth(100)]
    [NodeTint("#378c59")]
    public class StartNode : BaseNode
    {
        [Output, SerializeField] private int _exit;
        [Inject] private DialogueSystem _dialogueSystem;
        
        public override void Activate()
        {
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
}