using UnityEngine;

namespace Entities.NPC
{
    public class CalendarTrigger : MonoBehaviour
    {
        public void Take()
        {
            StartQuestTertilus.IsCalendarFound = true;
        }
    }
}
