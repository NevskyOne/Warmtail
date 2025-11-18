using System;
using Entities.PlayerScripts;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractionSystem : IAbility
{
    public bool Enabled { get; set; }
    
    [SerializeField] private float _interactionRadius = 2f;
    [SerializeField] private Vector3 _interactionOffset = Vector3.zero;
    
    private Player _player;
    
    [Inject]
    public void Construct(Player player, PlayerInput playerInput)
    {
        _player = player;
        playerInput.actions["LeftMouse"].performed += Interact;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (!Enabled) return;
        var colliders = Physics2D.OverlapCircleAll(_player.transform.position + _interactionOffset, _interactionRadius);
        
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
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
