using System.Collections.Generic;
using Data;
using Data.Player;
using Entities.PlayerScripts;
using Interfaces;
using Systems;
using Systems.DataSystems;
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
    public class DialogueVisuals : MonoBehaviour
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
        
        private DiContainer _diContainer;
        private DialogueSystem _system;
        private GlobalDataSystem _globalData;
        private PlayerInput _input;
        
        private Transform _playerTransform;
        public Transform NpcTransform { get; set; }
        public bool IsComplete { get; set; }
        public IPrintEffect Effect => _effect;

        [Inject]
        private void Construct(DiContainer container, PlayerInput input, DialogueSystem dialogueSystem, GlobalDataSystem globalData, Player player)
        {
            _diContainer = container;
            _input = input;
            _system = dialogueSystem;
            _globalData = globalData;
            _playerTransform = player.transform;
        }

        public void ShowDialogue()
        {
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxObject.SetActive(true);
                    break;
                case FrameType.Bubble:
                    _bubbleObject.SetActive(true);
                    break;
            }
        }
        
        public void HideDialogue()
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
        }
        
        public async void RequestNewLine(TextNode node)
        {
            IsComplete = false;
            var displayName = node.DisplayName == "" ? node.Character.ToString() : node.DisplayName;
            if (displayName == "Player")
            {
                displayName = _globalData.Get<SavablePlayerData>().PlayerName;
                _bubbleObject.transform.position = Camera.main!.WorldToScreenPoint(_playerTransform.position);
            }
            else
            {
                _bubbleObject.transform.position = Camera.main!.WorldToScreenPoint(NpcTransform.position);
            }
            switch (_frameType)
            {
                case FrameType.Box:
                    _boxName!.text = displayName;
                    var character = _holder.Characters.Find(x => x.Character == node.Character);
                    _boxImage!.sprite = character.EmotionSprites[node.Emotion];
                    
                    _boxText.alpha = 0f;
                    _boxText.text = node.Text;
                    IsComplete = await _effect.StartEffect(_boxText);
                    break;
                case FrameType.Bubble:
                    _bubbleName!.text = displayName;
                    
                    _bubbleText.alpha = 0f;
                    _bubbleText.text = node.Text;
                    IsComplete = await _effect.StartEffect(_bubbleText);
                    break;
            }
            
        }
        
        public async void ShowOptions(List<string> choices)
        {
            _input.SwitchCurrentActionMap("UI");
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
                    _bubbleObject.transform.position = Camera.main!.WorldToScreenPoint(_playerTransform.position);
                    _bubbleText.gameObject.SetActive(false);
                    _bubbleName.gameObject.SetActive(false);
                    _bubbleOptionsGroup.gameObject.SetActive(true);
                    break;
            }
        }

        public void ChooseOption(int i)
        {
            _input.SwitchCurrentActionMap("Dialogue");
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
            _system.SetNewNode(i.ToString());
            _system.ActivateNewNode();
        }
    }
}