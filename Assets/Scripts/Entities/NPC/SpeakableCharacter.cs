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
        private PlayerInput _input;
        
        [Inject]
        private void Construct(DialogueSystem dialogueSystem, PlayerInput input)
        {
            _dialogueSystem = dialogueSystem;
            _input = input;
        }

        private void OnEnable()
        {
            _input.actions.FindAction("E").performed += Interact;
        }

        private void OnDisable()
        {
             _input.actions.FindAction("E").performed -= Interact;
        }

        public void Interact() { }
        
        public void Interact(InputAction.CallbackContext ctx)
        {
            _dialogueSystem.StartDialogue(_graph, transform);
        }
    }
    
}