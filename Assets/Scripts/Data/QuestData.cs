using System;
using System.Collections.Generic;
using System.IO;
using Entities.Localization;
using TriInspector;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "QuestData", menuName = "Configs/QuestData")]
    public class QuestData : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Layer { get; private set; }
        [field: SerializeField] public int RequestsToDeactivate { get; private set; } = 1;
        [field: SerializeField] public List<Vector2> Targets { get; private set; }
    }
}