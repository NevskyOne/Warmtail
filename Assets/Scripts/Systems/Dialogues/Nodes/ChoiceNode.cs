using System.Collections.Generic;
using UnityEngine;

[NodeWidth(330)]
public class ChoiceNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    public List<string> Choices = new();

    public override void Activate()
    {
    
    }
}
