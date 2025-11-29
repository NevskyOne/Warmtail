using EasyTextEffects;
using EasyTextEffects.Effects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
{
    public class DialogueOptionUI : MonoBehaviour
    {
        [SerializeField] private Image _optionDot;
        [Inject] private DialogueVisuals _dialogueVisuals;
        
        public void ChooseOption()
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    _dialogueVisuals.ChooseOption(i);
                    break;
                }
            }
        }

        // public void HighlightOption(bool value)
        // {
        //     var color = _optionDot.color;
        //     color.a = value ? 1 : 0.5f;
        //     _optionDot.color = color;
        // }
    }
}