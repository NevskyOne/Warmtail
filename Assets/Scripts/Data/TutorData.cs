using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "TutorData", menuName = "Configs/TutorData")]
    public class TutorData : ScriptableObject
    {
        [field: Title("Sequence"),SerializeField] public List<SequenceElement> Sequence { get; private set; }
    }
}