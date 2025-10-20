using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Tutorial
{
    [Serializable]
    public class KeyBindTrigger: ITutorTrigger
    {
        [SerializeField] private InputActionReference _actionRef;
        public Action Event { get; set; }
        
        public void Activate()
        {
            _actionRef.action.performed += Invoke;
        }
        
        public void Deactivate()
        {
            _actionRef.action.performed -= Invoke;
        }

        private void Invoke(InputAction.CallbackContext _) => Event.Invoke();
    }
}