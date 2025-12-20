using Data;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class StartQuestAction : ISequenceAction
    {
        [SerializeField] private QuestData _quest;
        
        public void Invoke()
        {
            QuestSystem.StartQuest(_quest);
        }
	}
}