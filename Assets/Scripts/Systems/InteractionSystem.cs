using System;
using Entities.PlayerScripts;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractionSystem : IAbility
{
    public bool Enabled { get; set; }
    public Action StartAbility { get; set; }
    public Action UsingAbility { get; set; }
    public Action EndAbility { get; set; }
    
    [SerializeField] private float _interactionRadius = 2f;
    [SerializeField] private Vector3 _interactionOffset = Vector3.zero;
    
    private Player _player;
    
    [Inject]
    public void Construct(Player player, PlayerInput playerInput)
    {
        _player = player;
        playerInput.actions["LeftMouse"].started += _ => StartAbility?.Invoke();
        playerInput.actions["LeftMouse"].performed += Interact;
        playerInput.actions["LeftMouse"].canceled += _ => EndAbility?.Invoke();
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (!Enabled) return;
        var colliders = Physics2D.OverlapCircleAll(_player.Rigidbody.transform.position + _interactionOffset, _interactionRadius);
        
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
                UsingAbility?.Invoke();
                break;
            }
        }
    }
    
    public void FixedTick()
    {
        
    }
}
