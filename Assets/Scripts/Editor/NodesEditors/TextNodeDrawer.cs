#if UNITY_EDITOR
using UnityEditor;
using XNodeEditor;

namespace EditorOnly.NodesEditors
{
    [CustomNodeEditor(typeof(TextNode))]
    public class TextNodeDrawer : NodeEditor
    {
        private TextNode _textNode;
        private bool _showDisplayName;
        
        public override void OnBodyGUI()
        {
            if (_textNode == null)
            {
                _textNode = target as TextNode;
                _showDisplayName = _textNode?.DisplayName != "";
            }
            serializedObject.Update();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_entry"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_exit"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_textId"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_character"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_emotion"));

            var prevWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90;
            _showDisplayName = EditorGUILayout.Toggle("Override name", _showDisplayName);
            EditorGUIUtility.labelWidth = prevWidth;
            
            if (_showDisplayName)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("_displayName"));
            }
            
            
        }
    }
}
#endif