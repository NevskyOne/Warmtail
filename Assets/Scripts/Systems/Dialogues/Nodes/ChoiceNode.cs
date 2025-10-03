using System.Collections.Generic;
using UnityEngine;

namespace Systems.Dialogues.Nodes
{
    [NodeWidth(330)]
    public class ChoiceNode : BaseNode
    {
        [Input, SerializeField] private int _entry;
        public List<string> Choices;

        public override void Activate()
        {
        
        }
    }
}