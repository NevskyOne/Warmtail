using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Player;
using Entities.UI;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class QuestSystem
    {
        [SerializeField] private List<QuestData> _allQuests;
        private readonly Dictionary<QuestData, List<int>> _createdMarksInd = new();
        private GlobalData _globalData;
        private QuestVisuals _questVisuals;

        [Inject]
        private void Construct(GlobalData globalData, QuestVisuals visuals)
        {
            _globalData = globalData;
            _questVisuals = visuals;
            foreach (var id in _globalData.Get<SavablePlayerData>().QuestIds)
            {
                StartQuest(_allQuests.Find(x => x.Id == id.Key), id.Value);
            }
        }
        
        public void StartQuest(QuestData data, int questState = 0)
        {
            if(!_globalData.Get<SavablePlayerData>().QuestIds.Keys.Contains(data.Id))
                _globalData.Edit<SavablePlayerData>(playerData => playerData.QuestIds.Add(data.Id, questState));

            _questVisuals.SpawnQuest(data);
            for (int i = 0; i <= questState; i++)
            {
                data.Sequence[i].Action.Invoke();
            }
        }

        public void TryIterateSequence(QuestData data)
        {
            var currentState = _globalData.Get<SavablePlayerData>().QuestIds[data.Id];
            var sequence = data.Sequence[currentState];
            bool result = false;
            sequence.Tasks.ForEach(x => result = x.Completed || result);
            if(sequence.Tasks.Count == 0 || result)
            {
                sequence.Action.Invoke();
                _globalData.Edit<SavablePlayerData>(playerData => 
                    playerData.QuestIds.Add(data.Id, currentState + 1));
            }
        }
        
        public void EndQuest(QuestData data)
        {
            if(_globalData.Get<SavablePlayerData>().QuestIds.Keys.Contains(data.Id))
                _globalData.Edit<SavablePlayerData>(playerData=> playerData.QuestIds.Remove(data.Id));
            _questVisuals.DestroyQuest(data);
        }
    }
}