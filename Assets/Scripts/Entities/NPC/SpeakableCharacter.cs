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
    public class SpeakableCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraph _graph;
        [SerializeField] private List<UnityEvent> _actions = new();
        private DialogueSystem _dialogueSystem;
        private DialogueVisuals _visuals;
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem, DialogueVisuals visuals)
        {
            _dialogueSystem = dialogueSystem;
            _visuals = visuals;
        }
        
        public void Interact()
        {
            _dialogueSystem.StartDialogue(_graph, _visuals, this);
        }

        public void InvokeAction(int ind) => _actions[ind].Invoke();
    }
    
}