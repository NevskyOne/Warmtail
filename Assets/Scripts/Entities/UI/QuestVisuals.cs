using System.Collections.Generic;
using Data;
using Data.Player;
using Entities.Localization;
using TMPro;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class QuestVisuals : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField, Tooltip("x: horizontal\ny: top\nz:bottom")] private Vector3 _markOffset;
        [SerializeField] private Camera _cam;
        [Title("UI")]
        [SerializeField] private RectTransform _markPrefab;
        [SerializeField] private RectTransform _markHud;
        [SerializeField] private RectTransform _questPrefab;
        [SerializeField] private RectTransform _questHud;

        private readonly Dictionary<QuestData, List<MarkUIData>> _createdMarks = new();
        private readonly Dictionary<QuestData, GameObject> _createdQuests = new();
        
        private DiContainer _diContainer;
        private GlobalData _globalData;

        [Inject]
        private void Construct(DiContainer diContainer, GlobalData globalData)
        {
            _diContainer = diContainer;
            _globalData = globalData;
        }

        public void SpawnQuest(QuestData data)
        {
            var newQuest = _diContainer.InstantiatePrefab(_questPrefab, _questHud).transform;
            newQuest.GetChild(0).GetComponent<LocalizedText>().SetNewKey("quest_header_" + data.Id);
            newQuest.GetChild(1).GetComponent<LocalizedText>().SetNewKey("quest_desc_" + data.Id);
            var allQuestCount = 0;
            data.Sequence.ForEach(x => allQuestCount += x.Tasks.Count);
            var questState = _globalData.Get<SavablePlayerData>().QuestIds[data.Id];
            newQuest.GetChild(2).GetComponent<TMP_Text>().text = ((questState + 1)/allQuestCount).ToString();
            _createdQuests.Add(data, newQuest.gameObject);
            _createdMarks.Add(data, new());
        }
        
        public void DestroyQuest(QuestData data)
        {
            Destroy(_createdQuests[data]);
            DestroyMarks(data);
            _createdMarks.Remove(data);
            _createdQuests.Remove(data);
        }
        
        public void SpawnMarks(QuestData data, List<Vector2> marksPos)
        {
            foreach (var markPos in marksPos)
            {
                var newMark = Instantiate(_markPrefab, _markHud);
                _createdMarks[data].Add(new MarkUIData
                {
                    Object = newMark.gameObject,
                    WorldPos = markPos
                });
            }
        }

        public void DestroyMark(QuestData data, int index)
        {
            var mark = _createdMarks[data][index];
            Destroy(mark.Object);
            _createdMarks[data].RemoveAt(index);
        }

        private void DestroyMarks(QuestData data)
        {
            foreach (var mark in _createdMarks[data])
            {
                Destroy(mark.Object);
            }
        }
        
        public void Update()
        {
            foreach (var mark in _createdMarks)
            {
                CalculateMarksPositions(mark.Value);
            }
        }
        
        private void CalculateMarksPositions(List<MarkUIData> marks)
        {
            for (var i = 0; i < marks.Count; i++)
            {
                var screenPos = _cam.WorldToScreenPoint(marks[i].WorldPos);

                Vector2 newScreenPos = new Vector2(screenPos.x, screenPos.y);
                if (screenPos.x > Screen.width - Screen.width * _markOffset.x)
                {
                    newScreenPos.x = Screen.width - Screen.width * _markOffset.x;
                }
                else if (screenPos.x < Screen.width * _markOffset.x)
                {
                    newScreenPos.x = Screen.width * _markOffset.x;
                }
                
                if (screenPos.y > Screen.height - Screen.height * _markOffset.y)
                {
                    newScreenPos.y = Screen.height - Screen.height * _markOffset.y;
                }
                else if (screenPos.y < Screen.height * _markOffset.z)
                {
                    newScreenPos.y = Screen.height * _markOffset.z;
                }
                Vector2 toTarget = (Vector2)screenPos - newScreenPos;
                if (toTarget.sqrMagnitude > 0.0001f)
                {
                    float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                    marks[i].Object.transform.localRotation = Quaternion.Euler(0f, 0f, angle + 90);
                }
                marks[i].Object.transform.position = newScreenPos;
            }
        }
    }
}