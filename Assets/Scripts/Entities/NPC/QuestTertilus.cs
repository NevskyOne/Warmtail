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
        [SerializeField] private DialogueGraph _graph3;
        [SerializeField] private DialogueGraph _graph5;
        [SerializeField] private SpeakableCharacter _characterTertilus;
        [SerializeField] private ShoppingManager _shoppingManager;

        void Start()
        {
            int val = _globalData.Get<WorldData>().SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"];
            if (val == 3 || val == 4)
            {
                _characterTertilus.Graph = _graph3;
            }
            else if (val == 5)
            {
                _characterTertilus.Graph = _graph5;
                _shoppingManager.RaiseFriendshipToLevel(3);
                _shoppingManager.RaiseFriendship(1);
            }
        }

        public void EndQuest11()
        {
            Debug.Log("Ira 4");
            _globalData.Edit<WorldData>(data => data.SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] = 4);
        }

        public void StartQuest11()
        {
            Debug.Log("Ira 2");
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