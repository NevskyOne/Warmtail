using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Dialogues.Nodes
{
    [NodeWidth(330)]
    public class ConditionNode : BaseNode
    {
        [Input, SerializeField] private int _entry;
        [Output, SerializeField] private int _exit;
        public List<ConditionStruct> Conditions;

        public override void Activate()
        {
        
        }
    }

    [Serializable]
    public struct ConditionStruct
    {
        public string VarName;
        public ComparisonOperation Operation;
        public string Value;
    }

    public enum ComparisonOperation {Equals, NotEquals, Less, More}
}