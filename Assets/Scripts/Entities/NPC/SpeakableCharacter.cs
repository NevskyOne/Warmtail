using Interfaces;
using Systems.Dialogues;
using Systems.Dialogues.Nodes;
using UnityEngine;
using Zenject;

namespace Entities.NPC
{
    public class SpeakableCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraph _graph;
        private DiContainer _diContainer;
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public void Interact()
        {
            _diContainer.Inject(_graph.Current);
            _graph.Current.Activate();
        }
    }
    
}