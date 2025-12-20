using Data;
using Entities.Core;
using Entities.UI;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class MarkAction : ISequenceAction
    {
        [SerializeField] private bool _spawn;
        [SerializeField] private QuestData _questData;
        [SerializeField] private Vector2 _position;
        [SerializeField] private string _questVisualsId;
        
        public void Invoke()
        {
            var q = SavableObjectsResolver.FindObjectById<QuestVisuals>(_questVisualsId);
            if(_spawn) q.SpawnMarks(_questData, _position);
            else q.DestroyMark(_questData, _position);
        }
    }
}