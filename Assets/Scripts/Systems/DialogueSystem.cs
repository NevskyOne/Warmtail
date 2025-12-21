using System.Collections.Generic;
using Data;
using Data.Nodes;
using Interfaces;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems
{
    
    public class DialogueSystem
    {
        private RuntimeDialogueGraph _dialogueGraph;
        private DiContainer _diContainer;
        private ITextVisual _visuals;
        private PlayerInput _input;
        private string _prevActionMap;
        private Dictionary<string, RuntimeNode> _nodeLookup = new();
        private RuntimeNode _currentNode;
        
        public ITextVisual Visuals => _visuals;
        
        public RuntimeDialogueGraph DialogueGraph => _dialogueGraph;
        public IEventInvoker Character { get; private set; }

        [Inject]
        private void Construct(DiContainer container, PlayerInput input)
        {
            _diContainer = container;
            _input = input;
        }
        
        public void RequestNewNode()
        {
            if (_dialogueGraph != null && _currentNode!= null)
            {
                ActivateNewNode();
            }
        }
        
        public void StartDialogue(RuntimeDialogueGraph graph, ITextVisual visual, IEventInvoker character = null)
        {
            if(graph.EntryNodeId == null) return;
            graph.AllNodes.ForEach(x => _nodeLookup.Add(x.NodeId, x));
            _visuals = visual;
            _visuals.ShowVisuals();
            Character = character;
            _prevActionMap = "Player";
            _input.SwitchCurrentActionMap("Dialogue");
            _dialogueGraph = graph;
            _currentNode = _nodeLookup[_dialogueGraph.EntryNodeId];
            SetNewNode();
            ActivateNewNode();
        }

        public void SetNewNode(string portName = "out")
        {
            var nextNode = _currentNode.NextNodeIds.Find(x => x == portName);
            if(nextNode != null) _currentNode = _nodeLookup[nextNode];
            else EndDialogue();
        }

        public void ActivateNewNode()
        {
            _diContainer.Inject(_currentNode);
            _currentNode.Activate();
        }
        
        private void EndDialogue()
        {
            _nodeLookup.Clear();
            Character = null;
            _dialogueGraph = null;
            _visuals.HideVisuals();
            _input.SwitchCurrentActionMap(_prevActionMap);
        }
    }
}