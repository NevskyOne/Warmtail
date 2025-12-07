using UnityEngine;
using Data;
using Zenject;

namespace Entities.NPC
{
    public class StartQuestTertilus : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        public void StartQuest11()
        {
            _globalData.Edit<WorldData>(data => {
                data.SavableObjects["f84fdf28-aaad-4560-b7bb-c7dcf0f3f5fd"] = false;
                data.SavableObjects["ee7ebb47-6732-4f54-aa5c-b9884572c6a6"] = false;
                data.SavableObjects["a42432cc-751f-4437-9840-856c7420910d"] = false;
                data.SavableObjects["6e9af50b-9b91-42dd-8f59-4a48bddc8021"] = false;
                data.SavableObjects["05fdc1cf-2c88-45bf-a4d7-71637df9a8f4"] = true;
            });
        }
    }
}