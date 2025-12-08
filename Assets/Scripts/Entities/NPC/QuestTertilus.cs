using UnityEngine;
using Data;
using Zenject;
using Data.Nodes;
using XNode;

namespace Entities.NPC
{
    public class QuestTertilus : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        [SerializeField] private DialogueGraph _graphNext;
        [SerializeField] private SpeakableCharacter _characterTertilus;

        void Start()
        {
            int val = _globalData.Get<WorldData>().SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"];
            if (val == 3)
            {
                _characterTertilus.Graph = _graphNext;
            }
        }

        public void EndQuest11()
        {
            _globalData.Edit<WorldData>(data => data.SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] = 4);
            _globalData.Edit<NpcSpawnData>(data => data.CurrentHomeNpc = Characters.player);
        }

        public void StartQuest11()
        {
            _globalData.Edit<WorldData>(data => {
                data.SavableObjects["f84fdf28-aaad-4560-b7bb-c7dcf0f3f5fd"] = false;
                data.SavableObjects["ee7ebb47-6732-4f54-aa5c-b9884572c6a6"] = false;
                data.SavableObjects["a42432cc-751f-4437-9840-856c7420910d"] = false;
                data.SavableObjects["6e9af50b-9b91-42dd-8f59-4a48bddc8021"] = false;
                data.SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] = 2;
            });
        }
    }
}