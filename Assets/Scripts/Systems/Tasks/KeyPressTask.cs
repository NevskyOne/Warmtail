using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Tasks
{
    public class KeyPressTask : ITask
    {
        public bool Completed { get; set; }
        public Action OnComplete { get; set; }
        [SerializeField] private InputAction _action;

        public void Activate()
        {
            _action.performed += MarkComplete;
        }

        private void MarkComplete(InputAction.CallbackContext _)
        {
            Completed = true;
            OnComplete?.Invoke();
            _action.performed -= MarkComplete;
        }
    }
}