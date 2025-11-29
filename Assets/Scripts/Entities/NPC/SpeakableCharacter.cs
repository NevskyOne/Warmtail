using System.Collections.Generic;
using Data;
using Data.Nodes;
using Entities.Probs;
using Entities.UI;
using Interfaces;
using Systems;
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
        [SerializeField] private UnityEvent _warmAction = new();
        private DialogueSystem _dialogueSystem;
        private DialogueVisuals _visuals;
        private float _warmPercent;

        [field: SerializeField] public bool CanWarm { get; set; } = true;
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem, DialogueVisuals visuals)
        {
            _dialogueSystem = dialogueSystem;
            _visuals = visuals;
            Reset();
        }
        
        public void Interact()
        {
            if (!Graph) return;
            _dialogueSystem.StartDialogue(Graph, _visuals, this);
            Graph = null;
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
    }
    
}