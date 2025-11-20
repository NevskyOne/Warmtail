using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Entities.House
{
    public class DraggableObject : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private InputAction _leftClickAction;
        private bool _isMenuEnabled;

        public void Initialize(Sprite sprite, Vector2 scale)
        {
            _spriteRenderer.sprite = sprite;
            transform.localScale = scale;
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            EnableMenu();
        }
        
        void Start()
        {
            Debug.Log(InputSystem.actions);
            Debug.Log(InputSystem.actions.FindAction("HouseLeftMouse"));
            _leftClickAction = InputSystem.actions.FindAction("HouseLeftMouse");
        }
        private void Update()
        {
            if (_leftClickAction.WasPressedThisFrame())
            {
                DisableMenu();
            }
        }
        private void DisableMenu()
        {
            if (!_isMenuEnabled) return;
            _isMenuEnabled = false;
            Debug.Log("Disable");
        }
        private void EnableMenu()
        {
            if (_isMenuEnabled) return;
            _isMenuEnabled = true;
            Debug.Log("Enable");
        }
    }
}
