using Data;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entities.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class UnityEventTrigger : MonoBehaviour, IDeletable
    {
        [SerializeField] private bool _destroyAfter;
        [SerializeField] private UnityEvent _event;
        [Inject] private GlobalData _data;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _event.Invoke();
                if (_destroyAfter)
                {
                    ((IDeletable)this).Delete(_data, gameObject.GetEntityId());
                    Destroy(gameObject);
                }
            }
        }
    }
}