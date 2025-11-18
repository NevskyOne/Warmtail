using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractionSystem : IAbility
{
    public bool Enabled { get; set; }
    
    [SerializeField] private float _interactionRadius = 2f;
    [SerializeField] private Vector3 _interactionOffset = Vector3.zero;
    
    private MonoBehaviour _player;
    
    [Inject]
    public void Construct( PlayerInput playerInput)
    {
      
        playerInput.actions["Interact"].performed += Interact;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        Collider[] colliders = Physics.OverlapSphere(
            _player.transform.position + _interactionOffset, 
            _interactionRadius
        );
        
        foreach (var collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                break;
            }
        }
    }
    
    public void FixedTick()
    {
        
    }
}
