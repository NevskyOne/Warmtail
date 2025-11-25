using System.Collections.Generic;
using Data;
using Data.Nodes;
using Entities.UI;
using Interfaces;
using Systems;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entities.NPC
{
    public class SpeakableCharacter : MonoBehaviour, IInteractable, IWarmable
    {
        [field: SerializeField] public DialogueGraph Graph { get; set; }
        [SerializeField] private List<UnityEvent> _actions = new();
        [SerializeField, Range(0,1f)] private float _maxWarmPercent;
        [SerializeField] private UnityEvent _warmAction = new();
        private DialogueSystem _dialogueSystem;
        private DialogueVisuals _visuals;
        private float _warmPercent;
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem, DialogueVisuals visuals)
        {
            _dialogueSystem = dialogueSystem;
            _visuals = visuals;
            Reset();
        }
        
        public void Interact()
        {
            _dialogueSystem.StartDialogue(Graph, _visuals, this);
        }

        public void InvokeAction(int ind) => _actions[ind].Invoke();
        public void Warm()
        {
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