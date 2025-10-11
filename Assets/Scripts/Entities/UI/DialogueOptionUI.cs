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
        [SerializeField] private TextEffect _effect;
        [SerializeField] private TextEffectInstance _normalEffect;
        [SerializeField] private TextEffectInstance _highlightEffect;
        [Inject] private DialogueVisuals _dialogueVisuals;

        private void Start()
        {
            _effect.StartManualEffects();
        }
        
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

        public void HighlightOption(bool value)
        {
            var color = _optionDot.color;
            color.a = value ? 1 : 0.5f;
            _optionDot.color = color;
            
            _effect.globalEffects[1].effect = value ? _highlightEffect : _normalEffect;
            _effect.Refresh();
            _effect.StopOnStartEffects();
            _effect.StartManualEffects();
        }
    }
}