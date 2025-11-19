using System;
using Data.Nodes;
using Interfaces;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.NPC
{
    public class SpeakableCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraph _graph;
        private DialogueSystem _dialogueSystem;
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem)
        {
            _dialogueSystem = dialogueSystem;
        }
        
        public void Interact()
        {
            _dialogueSystem.StartDialogue(_graph, transform);
        }
    }
    
}