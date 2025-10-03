using Systems.Dialogues.Nodes;
using XNode;
using XNodeEditor;

namespace Systems.Dialogues.NodesEditors
{
    [CustomNodeEditor(typeof(ConditionNode))]
    public class ConditionNodeDrawer : NodeEditor
    {
        private ConditionNode _conditionNode;
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

            if (_conditionNode != null && _lastConditionsCount != _conditionNode.Conditions.Count)
            {
                for (int i = 0; i < _conditionNode.Conditions.Count; i++)
                {
                    _conditionNode.AddDynamicOutput(typeof(int),Node.ConnectionType.Multiple, Node.TypeConstraint.None, $"{i}");
                }

                _lastConditionsCount = _conditionNode.Conditions.Count;
            }
            
            foreach (var dynamicPort in target.DynamicPorts) {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
            serializedObject.Update();
        }
    }
}