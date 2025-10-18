using System;
using Data.Nodes;
using Entities.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems
{
    
    public class DialogueSystem
    {
        private DialogueGraph _dialogueGraph;
        private DiContainer _diContainer;
        private DialogueVisuals _visuals;
        private PlayerInput _input;

        private string _prevActionMap;
        

        [Inject]
        private void Construct(DiContainer container, PlayerInput input, DialogueVisuals dialogueVisuals)
        {
            _diContainer = container;
            _visuals = dialogueVisuals;
            _input = input;
            _input.actions.FindAction("Space").performed += RequestNewNode;
        }

        private void RequestNewNode(InputAction.CallbackContext _)
        {
            if (!_visuals.IsComplete)
            {
                _visuals.ChangeEffectSpeed();
                return;
            }

            if (_dialogueGraph != null && _dialogueGraph.Current != null)
            {
                ActivateNewNode();
            }
        }
        
        public void StartDialogue(DialogueGraph graph, Transform npcTransform)
        {
            _dialogueGraph = graph;
            _dialogueGraph.Current = _dialogueGraph.StartNode;
            _visuals.NpcTransform = npcTransform;
            SetNewNode();
            ActivateNewNode();
            _visuals.ShowDialogue();
            
            _prevActionMap = _input.currentActionMap.name;
            _input.SwitchCurrentActionMap("Dialogue");
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
            _dialogueGraph = null;
            _visuals.HideDialogue();
            _input.SwitchCurrentActionMap(_prevActionMap);
        }
    }
}