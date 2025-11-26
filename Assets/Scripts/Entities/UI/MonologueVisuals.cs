using System;
using Cysharp.Threading.Tasks;
using Data.Nodes;
using Entities.Localization;
using Interfaces;
using Systems;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Entities.UI
{
    public class MonologueVisuals : MonoBehaviour, ITextVisual
    {
        [SerializeField] private float _textFadeSpeed;
        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private RectTransform _textBounds;
        private RectTransform _currentText;
        private LocalizationManager _localizationManager;
        private DialogueSystem _dialogueSystem;
        private UIStateSystem _uiStateSystem;
        private bool _isEnded;

        [Inject]
        private void Construct(LocalizationManager localizationManager, DialogueSystem dialogueSystem, UIStateSystem uiStateSystem)
        {
            _localizationManager = localizationManager;
            _dialogueSystem = dialogueSystem;
            _uiStateSystem = uiStateSystem;
        }

        public void StartMonologue(DialogueGraph graph)
        {
            _dialogueSystem.StartDialogue(graph, this);
            ProcessDialogue();
        }
        
        public async void ProcessDialogue()
        {
            while(true){
                await UniTask.Delay(TimeSpan.FromSeconds(_textFadeSpeed));
                if(_isEnded) break;
                _dialogueSystem.RequestNewNode();
            }
        }
            
        
        public void ShowVisuals()
        {
            _isEnded = false;
            _currentText = Instantiate(_textPrefab, _textBounds).GetComponent<RectTransform>();
            _currentText.anchoredPosition = ChooseRandomPosition();
        }

        public void HideVisuals()
        {
            _isEnded = true;
            Destroy(_currentText.gameObject);
        }

        public void RequestNewLine(TextNode node)
        {
            _currentText.anchoredPosition = ChooseRandomPosition();
            _currentText.GetComponent<TMP_Text>().text = 
                _localizationManager.GetStringFromKey("cutscene_"+ _dialogueSystem.DialogueGraph.DialogueId+ "_" + node.TextId);
        }
        
        public async void RequestSingleLine(int id)
        {
            _currentText = Instantiate(_textPrefab, _textBounds).GetComponent<RectTransform>();
            _currentText.anchoredPosition = ChooseRandomPosition();
            _currentText.GetComponent<TMP_Text>().text = 
                _localizationManager.GetStringFromKey("fragment_" + id);
            await UniTask.Delay(TimeSpan.FromSeconds(_textFadeSpeed));
            Destroy(_currentText.gameObject);
        }


        private Vector2 ChooseRandomPosition()
        {
            return new Vector2(Random.Range(_textBounds.anchorMin.x, _textBounds.anchorMax.x),
                Random.Range(_textBounds.anchorMin.y, _textBounds.anchorMax.y));
        }
        
    }
}