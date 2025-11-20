using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        void Awake()
        {
            _inputActions.FindActionMap("House").Enable();
        }
        void OnDestroy()
        {
            _inputActions.FindActionMap("House").Disable();
        }
    }
}