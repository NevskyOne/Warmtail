using System.Collections.Generic;
using Systems;
using Unity.GraphToolkit.Editor;
using Zenject;

namespace Data.Nodes
{
    public class TextNode : RuntimeNode
    {
        public string Text { get; private set; }

        public Character Character { get; private set; }

        public CharacterEmotion Emotion { get; private set; }

        public string DisplayName { get; private set; }
    
        [Inject] private DialogueSystem _dialogueSystem;

        public override void Setup(INode node, Dictionary<INode, string> nodeIdMap)
        {
            var nextNode = node.GetOutputPortByName("out")?.firstConnectedPort;
            if(nextNode != null)
                NextNodeIds.Add(nodeIdMap[nextNode.GetNode()]);
            Character = NodePortHelper.GetPortValue<Character>(node.GetInputPortByName("Character"));
            Emotion = NodePortHelper.GetPortValue<CharacterEmotion>(node.GetInputPortByName("Emotion"));
            DisplayName = NodePortHelper.GetPortValue<string>(node.GetInputPortByName("Override Name"));
            Text = NodePortHelper.GetPortValue<string>(node.GetInputPortByName("Text"));
        }

        public override void Activate()
        {
            _dialogueSystem.Visuals.RequestNewLine(this);
            _dialogueSystem.SetNewNode();
        }
    }
}