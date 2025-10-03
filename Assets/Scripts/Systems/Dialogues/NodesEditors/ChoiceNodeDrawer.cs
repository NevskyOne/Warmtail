using Systems.Dialogues.Nodes;
using XNode;
using XNodeEditor;

namespace Systems.Dialogues.NodesEditors
{
    [CustomNodeEditor(typeof(ChoiceNode))]
    public class ChoiceNodeDrawer : NodeEditor
    {
        private ChoiceNode _choiceNode;
        private int _lastChoicesCount;
        
        public override void OnBodyGUI()
        {
            if (_choiceNode == null)
            {
                _choiceNode = target as ChoiceNode;
            }
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_entry"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Choices"));

            if (_choiceNode != null && _lastChoicesCount != _choiceNode.Choices.Count)
            {
                for (int i = 0; i < _choiceNode.Choices.Count; i++)
                {
                    _choiceNode.AddDynamicOutput(typeof(int),Node.ConnectionType.Multiple, Node.TypeConstraint.None, $"{i}");
                }

                _lastChoicesCount = _choiceNode.Choices.Count;
            }
            
            foreach (var dynamicPort in target.DynamicPorts) {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
            serializedObject.Update();
        }
    }
}