using System.Collections.Generic;
using System.Linq;
using Data;
using Entities.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entities.NPC
{
    public class QuestSystem : MonoBehaviour
    {
        [SerializeField, Tooltip("x: horizontal\ny: top\nz:bottom")] private Vector3 _offset;
        [SerializeField] private Camera _cam;
        [SerializeField] private RectTransform _markPrefab;
        [SerializeField] private RectTransform _markHud;
        [SerializeField] private RectTransform _questPrefab;
        [SerializeField] private RectTransform _questParent;
        private readonly Dictionary<QuestData, List<RectTransform>> _createdMarks = new();
        private readonly Dictionary<QuestData, List<int>> _createdMarksInd = new();
        private readonly Dictionary<QuestData, GameObject> _createdQuests = new();
        private readonly Dictionary<QuestData, UnityEvent> _createdEvents = new();
        private readonly Dictionary<QuestData, int> _createdRequests = new();
        
        [Inject] private LocalizationManager _localization;
        
        public void StartQuest(QuestData data)
        {
            if (_createdQuests.ContainsKey(data)) return;
            var newQuest = Instantiate(_questPrefab, _questParent);
            newQuest.GetChild(0).GetComponent<LocalizedText>().SetNewKey("quest_header_" + data.Id);
            newQuest.GetChild(1).GetComponent<LocalizedText>().SetNewKey("quest_desc_" + data.Id);
            _createdQuests.Add(data, newQuest.gameObject);

            _createdMarks.Add(data, new());

            foreach (var target in data.Targets)
            {
                var newMark = Instantiate(_markPrefab, _markHud);
                _createdMarks[data].Add(newMark);
            }
            _createdRequests.Add(data,0);
            _createdMarksInd.Add(data, new());
        }
        
        public void EndQuest(QuestData data)
        {
            if (!_createdQuests.ContainsKey(data)) return;
            _createdRequests[data] += 1;
            if (_createdRequests[data] < data.RequestsToDeactivate) return;
            foreach (var mark in _createdMarks[data])
            {
                Destroy(mark.gameObject);
            }

            if(_createdEvents.TryGetValue(data, out var @event))
                @event.Invoke();
            Destroy(_createdQuests[data]);
            _createdMarks.Remove(data);
            _createdQuests.Remove(data);
            _createdEvents.Remove(data);
            _createdMarksInd.Remove(data);
            _createdRequests.Remove(data);
        }

        public void Update()
        {
            foreach (var mark in _createdMarks)
            {
                CalculateMarksPositions(mark.Value, mark.Key);
            }
        }

        private void CalculateMarksPositions(List<RectTransform> marks, QuestData data)
        {
            for (var i = 0; i < marks.Count; i++)
            {
                if (_createdMarksInd[data].Contains(i)) continue;
                var screenPos = _cam.WorldToScreenPoint(data.Targets[i]);
                marks[i].LookAt(screenPos, Vector3.down);
                if (screenPos.x > Screen.width - _offset.x)
                {
                    screenPos.x = Screen.width - _offset.x;
                }
                else if (screenPos.x < _offset.x)
                {
                    screenPos.x = _offset.x;
                }
                
                if (screenPos.y > Screen.height - _offset.y)
                {
                    screenPos.y = Screen.height - _offset.y;
                }
                else if (screenPos.y < _offset.z)
                {
                    screenPos.y = _offset.z;
                }
                
                marks[i].anchoredPosition = screenPos;
            }
        }

        public void AddEvent(QuestData data, UnityEvent unityEvent)
        {
            _createdEvents.Add(data, unityEvent);
        }

        public void CloseMark(string data)
        {
            var (questId, index) = (data.Split("_")[0], int.Parse(data.Split("_")[1]));
            var quest = _createdMarks.Keys.First(x => x.Id.ToString() == questId);
            print(index);
            var mark = _createdMarks[quest][index];
            _createdMarksInd[quest].Add(index);
            Destroy(mark.gameObject);
            if (_createdMarks[quest].Count == _createdMarksInd[quest].Count)
            {
                _createdMarks[quest].Clear();
                EndQuest(quest);
            }
        }
    }
}