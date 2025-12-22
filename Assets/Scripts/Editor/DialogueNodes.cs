
using System;
using System.Collections.Generic;
using Data.Nodes;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor
{
    [Serializable]
    public class StartNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<int>("Dialogue Id").WithDefaultValue(0).Build();
            context.AddOutputPort("out").Build();
        }
    }
    
    [Serializable]
    public class TextNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();

            context.AddInputPort<Character>("Character").Build();
            context.AddInputPort<string>("Override Name").Build();
            context.AddInputPort<CharacterEmotion>("Emotion").Build();
            context.AddInputPort<string>("Text").Build();
        }
    }
    
    [Serializable]
    public class ChoiceNode : Node
    {
        private const string _optionId = "Port Count";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();

            var option = GetNodeOptionByName(_optionId);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<string>($"{i}").Build();
                context.AddOutputPort($"{i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(_optionId).WithDefaultValue(2).Delayed();
        }
    }

    [Serializable]
    public class ConditionNode : Node
    {
        private const string PortCountOptionId = "PortCount";
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();

            var option = GetNodeOptionByName(PortCountOptionId);
            option.TryGetValue(out int portCount);

            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<ConditionStruct>($"{i}").WithDisplayName($"{i}").Build();
                context.AddOutputPort($"{i}").WithDisplayName($"{i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(PortCountOptionId)
                .WithDefaultValue(2)
                .Delayed();
        }
#if UNITY_EDITOR
        [UnityEditor.CustomPropertyDrawer(typeof(SetStruct))]
        public class SetStructDrawer : UnityEditor.PropertyDrawer
        {
            public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
            {
                position.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                var varNameProp = property.FindPropertyRelative("VarName");
                var opProp = property.FindPropertyRelative("Operation");
                var valueProp = property.FindPropertyRelative("Value");

                float third = position.width / 3f;

                UnityEditor.EditorGUI.PropertyField(new Rect(position.x, position.y, third, position.height), varNameProp, GUIContent.none);
                UnityEditor.EditorGUI.PropertyField(new Rect(position.x + third, position.y, third, position.height), opProp, GUIContent.none);
                UnityEditor.EditorGUI.PropertyField(new Rect(position.x + 2*third, position.y, third, position.height), valueProp, GUIContent.none);
            }

            public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
            {
                return UnityEditor.EditorGUIUtility.singleLineHeight;
            }
        }
#endif
    }

    [Serializable]
    public class ActionNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();
            
            context.AddInputPort<int>("Event Index").Build();
        }
    }
    
    [Serializable]
    public class SetNode : Node
    {
        private const string _optionId = "Port Count";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();

            var option = GetNodeOptionByName(_optionId);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<SetStruct>($"{i}").Build();
                context.AddOutputPort($"{i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(_optionId).WithDefaultValue(2).Delayed();
        }
        
#if UNITY_EDITOR
        [UnityEditor.CustomPropertyDrawer(typeof(ConditionStruct))]
        public class ConditionStructDrawer : UnityEditor.PropertyDrawer
        {
            public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
            {
                position.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                var varNameProp = property.FindPropertyRelative("VarName");
                var opProp = property.FindPropertyRelative("Operation");
                var valueProp = property.FindPropertyRelative("Value");

                float third = position.width / 3f;

                UnityEditor.EditorGUI.PropertyField(new Rect(position.x, position.y, third, position.height), varNameProp, GUIContent.none);
                UnityEditor.EditorGUI.PropertyField(new Rect(position.x + third, position.y, third, position.height), opProp, GUIContent.none);
                UnityEditor.EditorGUI.PropertyField(new Rect(position.x + 2*third, position.y, third, position.height), valueProp, GUIContent.none);
            }

            public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
            {
                return UnityEditor.EditorGUIUtility.singleLineHeight;
            }
        }
#endif
    }
    
    [Serializable]
    public class EndNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
        }
    }
}