using System;
using System.Collections.Generic;
using Data;
using Entities.NPC;
using Interfaces;
using Systems.Dialogues.Nodes;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
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
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Object")] 
        private GameObject _boxObject;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Text")] 
        private TMP_Text _boxText;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Name")] 
        private TMP_Text _boxName;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Character Image")] 
        private Image _boxImage;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Options Group")]
        private Transform _boxOptionsGroup;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Box), LabelText("Option Prefab")]
        private TMP_Text _boxOptionsPrefab;
        
        [GroupNext("BubbleFrame")]
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Object")] 
        private GameObject _bubbleObject;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Text")] 
        private TMP_Text _bubbleText;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Name")] 
        private TMP_Text _bubbleName;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Options Group")]
        private Transform _bubbleOptionsGroup;
        [SerializeField, ShowIf(nameof(_frameType), FrameType.Bubble), LabelText("Option Prefab")]
        private TMP_Text _bubbleOptionsPrefab;
        [UnGroupNext] 
        
        [SerializeField] private SpeakableCharacter _npc;

        private DialogueGraph _dialogueGraph;
        private BaseNode _startNode;
        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer container, PlayerInput input)
        {
            _diContainer = container;
            
            input.actions["1"].performed += _ =>
            {
                _npc.Interact();
            };
            
            input.actions["Space"].performed += _ => IterateDialogue();
        }
        
        public void StartDialogue(DialogueGraph graph, BaseNode startNode)
        {
            _dialogueGraph = graph;
            _startNode = startNode;
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
                    _boxImage!.sprite = character.EmotionSprites[node.Emotion];
                    break;
                case FrameType.Bubble:
                    _bubbleText.text = node.Text;
                    _bubbleName!.text = node.DisplayName == ""? node.Character.ToString() : node.DisplayName;
                    break;
            }
            _effect.StartEffect();
        }

        public async void ShowOptions(List<string> choices)
        {
            foreach (var choiceStr in choices)
            {
                switch (_frameType)
                {
                    case FrameType.Box:
                        var boxObj = await InstantiateAsync(_boxOptionsPrefab, _boxOptionsGroup);
                        boxObj[0].text = choiceStr;
                        _diContainer.Inject(boxObj[0].gameObject.GetComponent<DialogueOptionUI>());
                        break;
                    case FrameType.Bubble:
                        var bubbleObj = await InstantiateAsync(_bubbleOptionsPrefab, _bubbleOptionsGroup);
                        bubbleObj[0].text = choiceStr;
                        _diContainer.Inject(bubbleObj[0].gameObject.GetComponent<DialogueOptionUI>());
                        break;
                }
            }
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxText.gameObject.SetActive(false);
                    _boxName.gameObject.SetActive(false);
                    _boxImage.gameObject.SetActive(false);
                    _boxOptionsGroup.gameObject.SetActive(true);
                    break;
                case FrameType.Bubble:
                    _bubbleText.gameObject.SetActive(false);
                    _bubbleName.gameObject.SetActive(false);
                    _bubbleOptionsGroup.gameObject.SetActive(true);
                    break;
            }
        }

        public void ChooseOption(int i)
        {
            switch (_frameType)
            {
                case FrameType.Box:
                    for (int j = _boxOptionsGroup.childCount - 1; j >= 0; j--)
                    {
                        Destroy(_boxOptionsGroup.GetChild(j).gameObject);
                    }
                    _boxText.gameObject.SetActive(true);
                    _boxName.gameObject.SetActive(true);
                    _boxImage.gameObject.SetActive(true);
                    _boxOptionsGroup.gameObject.SetActive(false);
                    break;
                case FrameType.Bubble:
                    for (int j = _bubbleOptionsGroup.childCount - 1; j >= 0; j--)
                    {
                        Destroy(_bubbleOptionsGroup.GetChild(j).gameObject);
                    }
                    _bubbleText.gameObject.SetActive(true);
                    _bubbleName.gameObject.SetActive(true);
                    _bubbleOptionsGroup.gameObject.SetActive(false);
                    break;
            }
            _dialogueGraph.Current = (BaseNode)_dialogueGraph.Current.GetOutputPort(i.ToString()).Connection.node;
            IterateDialogue();
        }
        
        private void OnDisable() => Dispose();

        public void Dispose()
        {
            EndDialogue();
        }
    }
}