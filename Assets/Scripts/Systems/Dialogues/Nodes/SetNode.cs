using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Dialogues.Nodes
{
    [NodeWidth(330)]
    public class SetNode : BaseNode
    {
        [Input, SerializeField] private int _entry;
        [Output, SerializeField] private int _exit;
        [SerializeField] private List<SetStruct> _variables;

        public override void Activate()
        {
        
        }
    }

    [Serializable]
    public struct SetStruct
    {
        public string VarName;
        public MathOperation Operation;
        public string Value;
    }

    public enum MathOperation {Assign, Add, Substruct, Multiply, Divide}
}