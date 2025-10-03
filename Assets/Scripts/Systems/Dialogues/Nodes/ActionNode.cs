using UnityEngine;
using UnityEngine.Events;

namespace Systems.Dialogues.Nodes
{
    [NodeWidth(290)]
    public class ActionNode : BaseNode
    {
        [Input, SerializeField] private int _entry;
        [Output, SerializeField] private int _exit;
        [SerializeField] private UnityEvent _event;

        public override void Activate()
        {
            _event.Invoke();
        }
    }
}
