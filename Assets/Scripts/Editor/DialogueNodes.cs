using System;
using Unity.GraphToolkit.Editor;

namespace Editor
{
    [Serializable]
    public class StartNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
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

            context.AddInputPort<Characters>("Character").Build();
            context.AddInputPort<string>("Text").Build();
        }
    }
    
    [Serializable]
    public class ChoiceNode : Node
    {
        
    }
    
    [Serializable]
    public class ConditionNode : Node
    {
        
    }
    
    [Serializable]
    public class ActionNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();
        }
    }
    
    [Serializable]
    public class SetNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();
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