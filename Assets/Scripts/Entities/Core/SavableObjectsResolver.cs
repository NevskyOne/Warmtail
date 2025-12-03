using Data;
using Entities.NPC;
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
            var changedNpc = _data.Get<WorldData>().SavableNpcState;

            foreach (var obj in FindObjectsByType<SavableStateObject>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                if (changed.ContainsKey(obj.Id))
                {
                    obj.gameObject.SetActive(changed[obj.Id]);
                }

                if (changedNpc.ContainsKey(obj.Id))
                {
                    ((SpeakableCharacter)obj).SavableState[changedNpc[obj.Id]].Invoke();
                }
            }
        }
    }
}