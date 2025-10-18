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
        private void Construct(DialogueSystem dialogueSystem, PlayerInput input)
        {
            _dialogueSystem = dialogueSystem;
            input.actions.FindAction("E").performed += _ => Interact();
        }
        
        public void Interact()
        {
            _dialogueSystem.StartDialogue(_graph, transform);
        }
    }
    
}