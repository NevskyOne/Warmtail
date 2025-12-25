using System.Collections.Generic;
using Systems;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using Zenject;

namespace Data.Nodes
{
    public class TextNode : RuntimeNode
    {
        [field: SerializeField] public string Text { get; private set; }

        [field: SerializeField] public Character Character { get; private set; }

        [field: SerializeField] public CharacterEmotion Emotion { get; private set; }

        [field: SerializeField] public string DisplayName { get; private set; }
    
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