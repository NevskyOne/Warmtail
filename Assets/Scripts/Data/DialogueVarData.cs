using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DialogueVarData : ISavableData
    {
        [SerializeField, TableList(Draggable = true, AlwaysExpanded = true)]
        public List<DialogueVar> Variables;
    }

    [Serializable]
    public class DialogueVar
    {
        public enum VarType {String, Bool, Int, Float}
        public VarType Type;
        public string Name;
        public string Value;
    }
}