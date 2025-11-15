using UnityEngine;
using TMPro;

public class LocalizatedText : MonoBehaviour
{
    [SerializeField] private string _key;
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = LocalizationManager.Instance.GetStringFromKey(_key);
    }
    
}
