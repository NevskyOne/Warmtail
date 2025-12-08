using UnityEngine;
using Zenject;
using Data;

namespace Entities.NPC
{
    public class TertiliusCalendar : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        [SerializeField] private GameObject _teriliusLateGame;
        [SerializeField] private GameObject _calendar;
        void Start()
        {
            if (_globalData.Get<WorldData>().SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] >= 1)
            {
                Destroy(_teriliusLateGame);
            }
            if (_globalData.Get<WorldData>().SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] == 2)
            {
                _globalData.Edit<WorldData>(data => data.SavableObjects["87687026-2670-4bf0-a0f9-42ba999dcd8d"] = true);
                _calendar.SetActive(true);
            }
        }
        public void Take()
        {
            _globalData.Edit<WorldData>(data => data.SavableNpcState["2bb32deb-1b68-477a-b458-2cdec049ce57"] = 3);
        }
    }
}
