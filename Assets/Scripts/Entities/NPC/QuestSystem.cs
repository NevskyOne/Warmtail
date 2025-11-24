using System.Collections.Generic;
using Data;
using Entities.Localization;
using TMPro;
using UnityEngine;
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
        private readonly Dictionary<QuestData, GameObject> _createdQuests = new();
        
        [Inject] private LocalizationManager _localization;
        
        public void StartQuest(QuestData data)
        {
            if (_createdQuests.ContainsKey(data)) return;
            var newQuest = Instantiate(_questPrefab, _questParent);
            newQuest.GetChild(0).GetComponent<TMP_Text>().text = _localization.GetStringFromKey(data.Header);
            newQuest.GetChild(1).GetComponent<TMP_Text>().text = _localization.GetStringFromKey(data.Description);
            _createdQuests.Add(data, newQuest.gameObject);

            _createdMarks.Add(data, new());
            foreach (var target in data.Targets)
            {
                var newMark = Instantiate(_markPrefab, _markHud);
                _createdMarks[data].Add(newMark);
            }
            
        }
        
        public void EndQuest(QuestData data)
        {
            if (!_createdQuests.ContainsKey(data)) return;
            foreach (var mark in _createdMarks[data])
            {
                Destroy(mark.gameObject);
            }
            Destroy(_createdQuests[data]);
            _createdMarks.Remove(data);
            _createdQuests.Remove(data);
        }

        public void Update()
        {
            foreach (var mark in _createdMarks)
            {
                CalculateMarksPositions(mark.Value, mark.Key.Targets);
            }
        }

        private void CalculateMarksPositions(List<RectTransform> marks, List<Vector2> positions)
        {
            for (var i = 0; i < marks.Count; i++)
            {
                var screenPos = _cam.WorldToScreenPoint(positions[i]);
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
    }
}