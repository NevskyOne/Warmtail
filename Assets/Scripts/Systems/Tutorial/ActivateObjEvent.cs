using System;
using Interfaces;
using UnityEngine;

namespace Systems.Tutorial
{
    [Serializable]
    public class ActivateObjEvent : ITutorEvent
    {
        [SerializeField] private GameObject _object;
        
        public void Activate()
        {
            _object.SetActive(true);
        }
        
        public void Deactivate()
        {
            _object.SetActive(false);
        }
    }
}