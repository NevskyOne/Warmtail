using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractionSystem : IAbility
{
    public bool Enabled { get; set; }
    
    private MonoBehaviour _player;
    private float _interactionRadius = 2f;
    
    [Inject]
    public void Construct( PlayerInput playerInput)
    {
      
        playerInput.actions["Interact"].performed += Interact;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        Collider[] colliders = Physics.OverlapSphere(
            _player.transform.position, 
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