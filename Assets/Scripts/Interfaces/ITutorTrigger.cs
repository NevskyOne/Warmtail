using System;

namespace Interfaces
{
    public interface ITutorTrigger
    {
        public Action Event { get; set; }

        public void Trigger()
        {
            Event.Invoke();
            Deactivate();
        }
        public void Activate();
        public void Deactivate();
    }
}