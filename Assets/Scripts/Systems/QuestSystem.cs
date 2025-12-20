using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Player;
using Entities.UI;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Systems
{
    public class QuestSystem
    {
        private GlobalData _globalData;
        private QuestVisuals _questVisuals;

        [Inject]
        private void Construct(GlobalData globalData, QuestVisuals visuals)
        {
            _globalData = globalData;
            _questVisuals = visuals;
            foreach (var id in _globalData.Get<SavablePlayerData>().QuestIds)
            {
                StartQuest(visuals.AllQuests.Find(x => x.Id == id.Key), id.Value);
            }
        }
        
        public void StartQuest(QuestData data, int questState = 0)
        {
            if(!_globalData.Get<SavablePlayerData>().QuestIds.Keys.Contains(data.Id))
                _globalData.Edit<SavablePlayerData>(playerData => playerData.QuestIds.Add(data.Id, questState));

            if (data.Scene != SceneManager.GetActiveScene().name) return;
            
            _questVisuals.SpawnQuest(data);
            for (int i = 0; i < questState; i++)
            {
                data.Sequence[i].Actions.ForEach(x => x.Invoke());
            }

            foreach (var task in data.Sequence[questState].Tasks)
            {
                task.OnComplete += () => TryIterateSequence(data);
            }
        }

        private void TryIterateSequence(QuestData data)
        {
            var currentState = _globalData.Get<SavablePlayerData>().QuestIds[data.Id];
   
            var element = data.Sequence[currentState];
            if (element.Tasks.Count != 0 && !element.Tasks.TrueForAll(x => x.Completed)) 
                return;
        
            element.Actions.ForEach(x => x.Invoke());

            if (currentState == data.Sequence.Count - 1)
            {
                EndQuest(data);
            }
            else
            {
                currentState++;
                _globalData.Edit<SavablePlayerData>(playerData =>
                    playerData.QuestIds.Add(data.Id, currentState));
                
                if (data.Sequence[currentState].Tasks.Count > 0)
                {
                    foreach (var task in data.Sequence[currentState].Tasks)
                    {
                        task.OnComplete += () => TryIterateSequence(data);
                    }
                }
                else
                    TryIterateSequence(data);
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