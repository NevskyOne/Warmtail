using Data;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class EndQuestAction : ISequenceAction
    {
       [SerializeField] private QuestData _quest;
       
       public void Invoke()
       {
           QuestSystem.EndQuest(_quest);
       }
    }
}