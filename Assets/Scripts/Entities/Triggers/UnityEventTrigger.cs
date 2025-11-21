using UnityEngine;
using UnityEngine.Events;

namespace Entities.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class UnityEventTrigger : MonoBehaviour
    {
        [SerializeField] private bool _destroyAfter;
        [SerializeField] private UnityEvent _event;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _event.Invoke();
                if(_destroyAfter) Destroy(gameObject);
            }
        }
    }
}