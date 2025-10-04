using System.Collections.Generic;
using System.Linq;
using XNode;
using XNodeEditor;

namespace EditorOnly.NodesEditors
{
    [CustomNodeEditor(typeof(ConditionNode))]
    public class ConditionNodeDrawer : NodeEditor
    {
        private ConditionNode _conditionNode;
        private List<NodePort> _portsList = new();
        private int _lastConditionsCount;
        
        public override void OnBodyGUI()
        {
            if (_conditionNode == null)
            {
                _conditionNode = target as ConditionNode;
            }
            
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_entry"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Conditions"));
            
            if (_lastConditionsCount != _conditionNode!.Conditions.Count)
            {
                _conditionNode.ClearDynamicPorts();
                _portsList.Clear();
                for (int i = 0; i < _conditionNode.Conditions.Count; i++)
                {
                    var port = _conditionNode.AddDynamicOutput(typeof(int),Node.ConnectionType.Multiple,
                        Node.TypeConstraint.None, $"{i}");
                    _portsList.Add(port);
                }
                _lastConditionsCount = _conditionNode.Conditions.Count;
            }
            
            foreach (var dynamicPort in _portsList.Where(dynamicPort => !NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)))
            {
                NodeEditorGUILayout.PortField(dynamicPort);
            }
        }
    }
}