using System.Collections.Generic;
using System.Linq;
using Entities.UI;
using Systems;
using Unity.GraphToolkit.Editor;
using Zenject;

namespace Data.Nodes
{
    public class ChoiceNode : RuntimeNode
    {
        public List<string> Choices { get; private set; } = new();
    
        [Inject] private DialogueVisuals _dialogueVisuals;

        public override void Setup(INode node, Dictionary<INode, string> nodeIdMap)
        {
            var inputs = node.GetInputPorts().ToArray();
            var outputs = node.GetOutputPorts().ToArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                Choices.Add(NodePortHelper.GetPortValue<string>(inputs[i]));
                var nextNode = outputs[i]?.firstConnectedPort;
                if (nextNode != null)
                {
                    NextNodeIds.Add(nodeIdMap[nextNode.GetNode()]);
                }
            }
        }

        public override void Activate()
        {
            _dialogueVisuals.ShowOptions(this, Choices.Count);
        }
    }
}
