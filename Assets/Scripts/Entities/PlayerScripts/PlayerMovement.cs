using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 0;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    private void FixedUpdate()
    {
        // Плавное ускорение и замедление
        if (moveInput.magnitude > 0.1f)
        {
            currentVelocity = Vector2.MoveTowards(
                currentVelocity, 
                moveInput.normalized * moveSpeed, 
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(
                currentVelocity, 
                Vector2.zero, 
                deceleration * Time.fixedDeltaTime
            );
        }
        
        rb.linearVelocity = currentVelocity;
    }
}
