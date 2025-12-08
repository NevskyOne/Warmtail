using UnityEngine;
using Zenject;
using Data;

namespace Entities.NPC
{
    public class TetriliusQuestShells : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        [SerializeField] private GameObject _teriliusLateGame;

        void Start()
        {
            if (_globalData.Get<WorldData>().SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] >= 5)
            {
                Destroy(_teriliusLateGame);
            }
        }

        public void TakeQuest()
        {
            _globalData.Edit<WorldData>(data => data.SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] = 5);
            Debug.Log("Ira 5");
            _globalData.Edit<NpcSpawnData>(data => data.CurrentHomeNpc = Characters.tertilus);
        }
    }
}
