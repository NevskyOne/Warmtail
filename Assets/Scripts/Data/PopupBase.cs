using System;
using PrimeTween;
using TMPro;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data
{
    [Serializable]
    public abstract class PopupBase
    {
        public PopupType Type { get; protected set; }
        [field:SerializeReference] public GameObject Prefab { get; private set; }
        public string Header { get; private set; }
        public string Text { get; private set; }
        [field:SerializeReference, Unit("Seconds")] public float AnimDuration { get; private set; }

        protected Transform _instanceTf;

        public PopupBase(string header, string text, GameObject prefab = null)
        {
            Header = header;
            Text = text;
            Prefab = prefab;
        }
        
        public virtual void Setup(PopupBase data, Transform parent)
        {
            Prefab ??= data.Prefab;
            AnimDuration = data.AnimDuration;
            _instanceTf = Object.Instantiate(Prefab, parent).transform;
            Tween.Scale(_instanceTf, 0, 1, AnimDuration);
            _instanceTf.GetChild(0).GetComponent<TMP_Text>().text = Header;
            _instanceTf.GetChild(1).GetComponent<TMP_Text>().text = Text;
        }

        public async void ClosePopup()
        {
            await Tween.Scale(_instanceTf, 1, 0, AnimDuration);
            Object.Destroy(_instanceTf?.gameObject);
        }
    }
    
    public enum PopupType {Notification, Modular, Overlapping}
}