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
        [SerializeField] private List<ConditionStruct> _conditions = new();
        private const string _optionId = "Port Count";
        public IReadOnlyList<ConditionStruct> Conditions => _conditions;

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
            var option = GetNodeOptionByName(_optionId);
            option.TryGetValue(out int portCount);

            EnsureConditionsCount(portCount);

            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort($"{i}").Build();
                context.AddOutputPort($"{i}").Build();
            }
        }
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(_optionId).WithDefaultValue(2).Delayed();
        }
        private void EnsureConditionsCount(int count)
        {
            while (_conditions.Count < count)
                _conditions.Add(new ConditionStruct());

            while (_conditions.Count > count)
                _conditions.RemoveAt(_conditions.Count - 1);
        }
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