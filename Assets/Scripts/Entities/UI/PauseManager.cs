using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool pauseOnStart = false;
    
    private bool isPaused = false;
    private PlayerInput playerInput;
    
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }
    
    private void Start()
    {
        if (pauseOnStart)
        {
            Pause();
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        
        // Отключаем игровой ввод
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }
    
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        // Включаем игровой ввод
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
    
    public bool IsPaused => isPaused;
}