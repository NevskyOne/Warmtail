using Data;
using UnityEngine;
using Zenject;

namespace Entities.Probs
{
    public class SavableStateObject : MonoBehaviour
    {
        private int _id;
        private static int _lastId = 1;
        [Inject] protected GlobalData _globalData;
        
        public int Id => _id;

        private void Awake()
        {
            _id = _lastId++;
        }
        
        protected void ChangeState(bool active)
        {
            _globalData.Edit<WorldData>(worldData => worldData.SavableObjects.Add(_id, active));
            gameObject.SetActive(active);
        }
    }
}