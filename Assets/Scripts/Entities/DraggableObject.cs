using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

// namespace Entities.House
// {
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private GameObject _buttonsForConfirmed;
        [SerializeField] private GameObject _buttonsForEditing;
        [SerializeField] private HouseItemData _houseItemData;
        private InputAction _leftClickAction;
        private bool _isMenuEnabled;
        private bool _isConfirmed = true;
        private bool _isClickedNow;
        private int _thisItemId;

        void Awake()
        {
            PlacementSystem.OnApplyedAll += ApplyPlacement;
            PlacementSystem.OnCanceledAll += CancelPlacement;
            _leftClickAction = InputSystem.actions.FindAction("HouseLeftMouse");
        }
        void OnDestroy()
        {
            PlacementSystem.OnApplyedAll -= ApplyPlacement;
            PlacementSystem.OnCanceledAll -= CancelPlacement;
        }
        [Inject]
        private void Construct(PlacementSystem placementSystem)
        {
            _thisItemId = placementSystem.CountItemOnTheScene;
            placementSystem.CountItemOnTheScene ++;
            placementSystem.AddEditingItem(_houseItemData, transform.position);
        }
        public void Initialize(bool isConfirmed)
        {
            _isConfirmed = isConfirmed;
        }
        private void Update()
        {
            if (_leftClickAction.WasPressedThisFrame())
            {
                if (!_isClickedNow)
                    Invoke("DisableMenu", 0.1f);
            }
        }
        
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            _isClickedNow = true; 
            EnableMenu();
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            _isClickedNow = false;
        }
        private void DisableMenu()
        {
            if (!_isMenuEnabled) return;
            _isMenuEnabled = false;
            _buttonsForConfirmed.SetActive(false);
            _buttonsForEditing.SetActive(false);
        }
        private void EnableMenu()
        {
            if (_isMenuEnabled) return;
            _isMenuEnabled = true;
            if (_isConfirmed)
                _buttonsForConfirmed.SetActive(true);
            else
                _buttonsForEditing.SetActive(true);
        }

        public void ApplyPlacement()
        {
            if (_isConfirmed) return;
            _isConfirmed = true;
        }
        public void CancelPlacement()
        {
            if (_isConfirmed) return;
            Destroy(gameObject);
        }
        public void RemoveObject()
        {
            Destroy(gameObject);
        }
    }
// }
