using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IEventInvoker
    {
        public List<UnityEvent> Actions { get; set; }
        public void InvokeEvent(int ind) => Actions[ind].Invoke();
    }
}