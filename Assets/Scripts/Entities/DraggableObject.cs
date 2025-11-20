using UnityEngine;

namespace Entities.House
{
    public class DraggableObject : MonoBehaviour
    {
        private Vector2 _pointerDownPosition;
        
        public void PointerDownItem()
        {
            _pointerDownPosition = transform.position;
            Debug.Log(_pointerDownPosition);
        }
        public void PointerUpItem()
        {
        }
        public void HoldItem()
        {
        }
    }
}
