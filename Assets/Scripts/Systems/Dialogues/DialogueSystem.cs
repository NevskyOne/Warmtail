using System;
using Data;
using Interfaces;
using Systems.Dialogues.Nodes;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Systems.Dialogues
{
    public enum FrameType{Box, Bubble}
    
    [DeclareBoxGroup("BoxFrame")]
    [DeclareBoxGroup("BubbleFrame")]
    public class DialogueSystem : MonoBehaviour, IDisposable
    {
        [Title("Settings")]
        [SerializeReference] private CharacterHolder _holder;
        [SerializeReference] private IPrintEffect _effect;
        [SerializeReference] private FrameType _frameType;
        
        [GroupNext("BoxFrame")]
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Object")] private GameObject _boxObject;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Text")] private TMP_Text _boxText;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Name")] private TMP_Text _boxName;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Character Image")] private Image _boxImage;
        [GroupNext("BubbleFrame")]
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Object")] private GameObject _bubbleObject;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Text")] private TMP_Text _bubbleText;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Name")] private TMP_Text _bubbleName;
        [UnGroupNext] 
        
        [SerializeField] private DialogueGraph _dialogueGraph;
        private BaseNode _startNode;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer container, PlayerInput input)
        {
            _diContainer = container;
            _startNode = _dialogueGraph.Current;
            _diContainer.Inject(_dialogueGraph.Current);
            _dialogueGraph.Current.Activate();
            input.actions["Space"].performed += _ => IterateDialogue();
        }
        
        public void StartDialogue(DialogueGraph graph)
        {
            _dialogueGraph = graph;
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxObject.SetActive(true);
                    break;
                case FrameType.Bubble:
                    _bubbleObject.SetActive(true);
                    break;
            }

            IterateDialogue();
        }
        
        public void EndDialogue()
        {
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxObject.SetActive(false);
                    break;
                case FrameType.Bubble:
                    _bubbleObject.SetActive(false);
                    break;
            }
            _dialogueGraph.Current = _startNode;
        }

        public void IterateDialogue()
        {
            _diContainer.Inject(_dialogueGraph.Current);
            _dialogueGraph.Current.Activate();
        }
        
        public void RequestNewLine(TextNode node)
        {
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxText.text = node.Text;
                    _boxName!.text = node.DisplayName == ""? node.Character.ToString() : node.DisplayName;
                    var character = _holder.Characters.Find(x => x.Character == node.Character);
                    _boxImage!.sprite = character.Sprites[character.Emotions.IndexOf(node.Emotion)];
                    break;
                case FrameType.Bubble:
                    _bubbleText.text = node.Text;
                    _bubbleName!.text = node.DisplayName == ""? node.Character.ToString() : node.DisplayName;
                    break;
            }
            _effect.StartEffect();
        }

        private void OnDisable() => Dispose();
        public void Dispose()
        {
            EndDialogue();
        }
    }
}