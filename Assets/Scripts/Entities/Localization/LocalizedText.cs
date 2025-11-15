using TMPro;
using UnityEngine;
using Zenject;

namespace Entities.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        private TMP_Text Text => GetComponent<TMP_Text>();
    
        [Inject]
        private void Construct(LocalizationManager localization)
        {
            Text.text = localization.GetStringFromKey(_key);
        }
    }
}
