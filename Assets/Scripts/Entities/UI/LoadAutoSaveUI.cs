using Data;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class LoadAutoSaveUI : MonoBehaviour
    {
        [Inject] private GlobalData _globalData;
        public void Load() => _globalData.LoadAutoSave();
    }
}