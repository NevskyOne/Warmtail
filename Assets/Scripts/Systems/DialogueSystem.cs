using Data.Nodes;
using Entities.NPC;
using Entities.UI;
using Interfaces;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems
{
    
    public class DialogueSystem
    {
        private DialogueGraph _dialogueGraph;
        private DiContainer _diContainer;
        private ITextVisual _visuals;
        private PlayerInput _input;

        private string _prevActionMap;
        public DialogueGraph DialogueGraph => _dialogueGraph;
        public SpeakableCharacter Character { get; private set; }

        [Inject]
        private void Construct(DiContainer container, PlayerInput input)
        {
            _diContainer = container;
            _input = input;
        }
        
        public void RequestNewNode()
        {
            if (_dialogueGraph != null && _dialogueGraph.Current != null)
            {
                ActivateNewNode();
            }
        }
        
        public void StartDialogue(DialogueGraph graph, ITextVisual visual, SpeakableCharacter character = null)
        {
            Character = character;
            _prevActionMap = "Player";
            _input.SwitchCurrentActionMap("Dialogue");
            
            _dialogueGraph = graph;
            _dialogueGraph.Current = _dialogueGraph.StartNode;
            SetNewNode();
            ActivateNewNode();
            _visuals = visual;
            _visuals.ShowVisuals();
        }

        public void SetNewNode(string portName = "_exit")
        {
            _dialogueGraph.Current = (BaseNode)_dialogueGraph.Current.GetOutputPort(portName).Connection.node;
        }

        public void ActivateNewNode()
        {
            _diContainer.Inject(_dialogueGraph.Current);
            _dialogueGraph.Current.Activate();
        }
        
        public void EndDialogue()
        {
            Character = null;
            _dialogueGraph = null;
            _visuals.HideVisuals();
            _input.SwitchCurrentActionMap(_prevActionMap);
        }
    }
}