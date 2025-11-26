using Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
{
    public class ChangeDialogueVarUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _varName;
        [field: SerializeField] public string VarValue { get; set; }
        
        [Inject]
        private void Construct(GlobalData data)
        {
            _button.onClick.AddListener(() => data.Edit<DialogueVarData>(vars =>
            {
                vars.Variables.Find(x => x.Name == _varName).Value = VarValue;
            }));
        }
    }
}