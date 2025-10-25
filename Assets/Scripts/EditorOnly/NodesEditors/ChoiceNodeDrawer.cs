#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using XNode;
using XNodeEditor;

namespace EditorOnly.NodesEditors
{
    [CustomNodeEditor(typeof(ChoiceNode))]
    public class ChoiceNodeDrawer : NodeEditor
    {
        private ChoiceNode _choiceNode;
        private List<NodePort> _portsList = new();
        private int _lastChoicesCount;
        
        public override void OnBodyGUI()
        {
            if (_choiceNode == null)
            {
                _choiceNode = target as ChoiceNode;
                _lastChoicesCount = _choiceNode.Choices.Count;
                _portsList = _choiceNode.DynamicOutputs.ToList();
                _portsList.Sort((a,b) => int.Parse(a.fieldName).CompareTo(int.Parse(b.fieldName)));
            }
            
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_entry"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Choices"));
            
            if (_lastChoicesCount != _choiceNode!.Choices.Count)
            {
                _choiceNode.ClearDynamicPorts();
                _portsList.Clear();
                for (int i = 0; i < _choiceNode.Choices.Count; i++)
                {
                    var port = _choiceNode.AddDynamicOutput(typeof(int),Node.ConnectionType.Multiple,
                        Node.TypeConstraint.None, $"{i}");
                    _portsList.Add(port);
                }

                _lastChoicesCount = _choiceNode.Choices.Count;
            }
            
            foreach (var dynamicPort in _portsList.Where(dynamicPort => !NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)))
            {
                NodeEditorGUILayout.PortField(dynamicPort);
            }
        }
    }
}
#endif