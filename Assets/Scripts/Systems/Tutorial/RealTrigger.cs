using System;
using Interfaces;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Systems.Tutorial
{
    [Serializable]
    public class RealTrigger : ITutorTrigger
    {
        [SerializeField] private GameObject _target;
        public Action Event { get; set; }
        
        public void Activate()
        {
            _target.OnTriggerEnter2DAsObservable().Subscribe(_ => ((ITutorTrigger)this).Trigger()).AddTo(_target.gameObject);
        }

        public void Deactivate()
        {
        }
    }
}