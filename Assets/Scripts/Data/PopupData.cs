using System;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Data
{
    [Serializable]
    public abstract class PopupBase
    {
        public PopupType Type { get; protected set; }
        [ShowInInspector] public GameObject Prefab { get; private set; }
        public string Header { get; private set; }
        public string Text { get; private set; }

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
            _instanceTf = Object.Instantiate(data.Prefab, parent).transform;
            _instanceTf.GetChild(1).GetComponent<TMP_Text>().text = Header;
            _instanceTf.GetChild(2).GetComponent<TMP_Text>().text = Text;
        }
    }
    
    [Serializable]
    public class NotificationPopup : PopupBase
    {
        [ShowInInspector] public Sprite Icon { get; private set; }
        [ShowInInspector] public float Duration { get; private set; }
        
        public NotificationPopup(string header, string text, float duration, GameObject prefab = null, Sprite icon = null) : base(header, text, prefab)
        {
            Type = PopupType.Notification;
            Icon = icon;
            Duration = duration;
        }
        
        public override void Setup(PopupBase data, Transform parent)
        {
            base.Setup(data, parent);
            Icon ??= ((NotificationPopup)data).Icon;
            _instanceTf.GetChild(3).GetComponent<Image>().sprite = Icon;
        }
    }
    
    [Serializable]
    public class ModularPopup : PopupBase
    {
        
        public ModularPopup(string header, string text, GameObject prefab = null, Sprite icon = null) : base(header, text, prefab)
        {
            Type = PopupType.Modular;
        }
        
        public override void Setup(PopupBase data, Transform parent)
        {
            base.Setup(data, parent);
        }
    }
    
    public enum PopupType {Notification, Modular, Overlapping}
}