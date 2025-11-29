using Data;
using Entities.Probs;
using UnityEngine;
using Zenject;

namespace Entities.Core
{
    public class SavableObjectsResolver : MonoBehaviour
    {
        [Inject] private GlobalData _data;

        private void Start()
        {
            var changed = _data.Get<WorldData>().SavableObjects;

            foreach (var obj in FindObjectsByType<SavableStateObject>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                print(obj.Id);
                if (changed.ContainsKey(obj.Id))
                {
                    obj.gameObject.SetActive(changed[obj.Id]);
                }
            }
        }
    }
}