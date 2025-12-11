using System.Collections.Generic;
using System.Globalization;
using Data.Nodes;
using Entities.Probs;
using Entities.UI;
using Interfaces;
using Systems;
using Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entities.NPC
{
    public class SpeakableCharacter : SavableStateObject, IInteractable, IWarmable, IEventInvoker
    {
        [field: SerializeField] public DialogueGraph Graph { get; set; }
        [field: SerializeField] public List<UnityEvent> Actions { get; set; }
        [SerializeField, Range(0,1f)] private float _maxWarmPercent;
        [field: SerializeField] public bool CanWarm { get; set; } = true;
        [SerializeField] private UnityEvent _warmAction = new();
        [SerializeField] private bool _isInHome = false;
        [field: SerializeField] public List<UnityEvent> SavableState { get; private set; }
        private DialogueSystem _dialogueSystem;
        private DialogueVisuals _visuals;
        private float _warmPercent;
        private new GlobalData _globalData;
        private UIStateSystem _uiStateSystem;
        
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem, DialogueVisuals visuals, GlobalData globalData, UIStateSystem uiStateSystem)
        {
            _dialogueSystem = dialogueSystem;
            _visuals = visuals;
            _globalData = globalData;
            _uiStateSystem = uiStateSystem;
            Reset();
        }
        
        public void Interact()
        {
            if (!Graph || (_uiStateSystem && _uiStateSystem.CurrentState == UIState.Shop)) return;
            _dialogueSystem.StartDialogue(Graph, _visuals, this);
            if (!_isInHome) Graph = null;
        }
        
        public void Warm()
        {
            if (!CanWarm) return;
            _warmPercent -= 0.1f;
            if (_warmPercent <= 0)
                WarmExplosion();
        }

        public void WarmExplosion()
        {
            _warmAction.Invoke();
        }

        public void Reset()
        {
            _warmPercent = _maxWarmPercent;
        }

        public void SetPosition(string pos)
        {
            var (x, y) = (float.Parse(pos.Split(' ')[0], CultureInfo.InvariantCulture),
                float.Parse(pos.Split(' ')[1], CultureInfo.InvariantCulture));
            transform.position = new Vector2(x,y);
        }

        public void AddNpcToHome(int character)
        {
            _globalData.Edit<NpcSpawnData>(data => data.CurrentHomeNpc = (Characters)character);
        }
    }
    
}
