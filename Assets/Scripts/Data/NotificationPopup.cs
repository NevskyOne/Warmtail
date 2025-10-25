using System;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    
    [Serializable]
    public class NotificationPopup : PopupBase
    {
        [field:SerializeReference] public Sprite Icon { get; private set; }
        [field:SerializeReference, Unit("Milliseconds")] public int Duration { get; private set; }
        
        public NotificationPopup(string header, string text, int duration, GameObject prefab = null, Sprite icon = null) : base(header, text, prefab)
        {
            Type = PopupType.Notification;
            Icon = icon;
            Duration = duration;
        }
        
        public override void Setup(PopupBase data, Transform parent)
        {
            base.Setup(data, parent);
            Icon ??= ((NotificationPopup)data).Icon;
            _instanceTf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Icon;
            if (Duration > 0) AutoClose();
        }

        private async void AutoClose()
        {
            await UniTask.Delay(Duration);
            ClosePopup();
        }
    }
}