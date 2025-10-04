using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class DialogueOptionUI : MonoBehaviour
    {
        [Inject] private DialogueSystem _dialogueSystem;

        public void ChooseOption()
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    _dialogueSystem.ChooseOption(i);
                    break;
                }
            }
        }
    }
}