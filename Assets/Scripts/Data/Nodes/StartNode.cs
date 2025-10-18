using UnityEngine;

[NodeWidth(100)]
[NodeTint("#378c59")]
public class StartNode : BaseNode
{
    [Output, SerializeField] private int _exit;
    public override void Activate() { }
}


