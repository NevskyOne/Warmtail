using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Data.House;
using Zenject;
using Systems;

namespace Entities.House
{
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private GameObject _buttonsForConfirmed;
        [SerializeField] private GameObject _buttonsForEditing;
        [SerializeField] private GameObject _child;
        [SerializeField] private HouseItemData _houseItemData;
        private PlacementSystem _placementSystem;
        private InputAction _leftClickAction;
        private BoxCollider _boxCollider;
        private Vector2 _posObjectOnPointerDown;
        private Vector2 _posMouseOnPointerDown;
        private Vector2 _posObjectOnConfirmedState;
        private bool _isClickedNow;
        private bool _isPlacementing;
        private bool _isMenuEnabled;
        private bool _isConfirmed = true;
        private int _thisItemId;

        void Awake()
        {
            PlacementSystem.OnApplyedAll += ApplyEditing;
            PlacementSystem.OnCanceledAll += CancelEdited;
            
            _boxCollider = GetComponent<BoxCollider>();
        }
        void OnDestroy()
        {
            PlacementSystem.OnApplyedAll -= ApplyEditing;
            PlacementSystem.OnCanceledAll -= CancelEdited;
        }
        [Inject]
        private void Construct(PlacementSystem placementSystem, PlayerInput input)
        {
            _leftClickAction = input.actions.FindAction("LeftMouse");
            _placementSystem = placementSystem;
            _thisItemId = placementSystem.CountItemOnTheScene;
            placementSystem.CountItemOnTheScene ++;
        }
        public void Initialize(bool isConfirmed)
        {
            _posObjectOnConfirmedState = (isConfirmed ? transform.position : Vector2.positiveInfinity);
            _isConfirmed = isConfirmed;
        }
        private void Update()
        {
            if (_leftClickAction.WasReleasedThisFrame())
            {
                if (!_isClickedNow)
                    DisableMenu();
                _isClickedNow = false;
            }
            if (_isPlacementing)
            {
                Vector2 v2 = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                transform.position = _posObjectOnPointerDown + (v2 - _posMouseOnPointerDown);
            }
        }
        
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            _isPlacementing = true;
            _posMouseOnPointerDown = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _posObjectOnPointerDown = transform.position;

            _isClickedNow = true; 
            EnableMenu();
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            Vector2 pos = transform.position;
            if (_posObjectOnConfirmedState != pos) {
                if (_isConfirmed) DisableMenu();
                _isConfirmed = false;
            }
            DisableMenu();
            EnableMenu();
            _isPlacementing = false;
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

        public void ApplyEditing()
        {
            if (_isConfirmed) return;
            if (!_boxCollider.enabled) 
            {
                Destroy(gameObject);
                _placementSystem.RemoveEditingItem(_houseItemData, _posObjectOnConfirmedState);
            }
            else
            {
                if (_posObjectOnConfirmedState.x == Vector2.positiveInfinity.x)
                    _placementSystem.AddEditingItem(_houseItemData, transform.position);
                else
                    _placementSystem.ReplaceEditingItem(_houseItemData, _posObjectOnConfirmedState, transform.position);
                _posObjectOnConfirmedState = transform.position;
                _isConfirmed = true;
            }
        }
        public void CancelEdited()
        {
            if (_isConfirmed) return;
            if (_posObjectOnConfirmedState.x == Vector2.positiveInfinity.x) Destroy(gameObject);
            else {
                _isConfirmed = true;
                transform.position = _posObjectOnConfirmedState;
                _child.SetActive(true);
                _boxCollider.enabled = true;
            }
        }
        public void RemoveObject()
        {
            _isConfirmed = false;
            _child.SetActive(false);
            _boxCollider.enabled = false;
        }
    }
}
