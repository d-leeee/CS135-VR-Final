using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button rulesButton;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject mainMenuPanel;

    public InputActionReference buttonPressB;

    private void Start()
    {
        Debug.Log("=== MainMenu Start() called ===");
        
        // Pause time when menu appears
        Time.timeScale = 0f;
        Debug.Log("Game paused - waiting for menu input");

        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }

        // Register button click listeners
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
            Debug.Log("Play button setup");
        }
        else
        {
            Debug.LogError("Play Button NOT assigned!");
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            Debug.Log("Quit button setup");
        }
        else
        {
            Debug.LogError("Quit Button NOT assigned!");
        }
        
        if (rulesButton != null)
        {
            rulesButton.onClick.AddListener(ToggleRules);
            Debug.Log("Rules button setup");
        }
        else
        {
            Debug.LogError("Rules Button NOT assigned!");
        }
    }

    private void PlayGame()
    {
        Debug.Log("===== PLAYGAME CALLED =====");
        Debug.Log("Starting game...");
        Pipes.speed = 5f;
        Time.timeScale = 1f;
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); // Hide the menu so they can play!
        }
    }

    private void QuitGame()
    {
        Debug.Log("===== QUITGAME CALLED =====");
        Debug.Log("Quitting game...");
        Time.timeScale = 1f;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void ToggleRules()
    {
        Debug.Log("===== TOGGLERULES CALLED =====");
        
        if (rulesPanel != null)
        {
            bool isRulesOpen = !rulesPanel.activeSelf;
            buttons.SetActive(!isRulesOpen); 
            rulesPanel.SetActive(isRulesOpen);
            
            Debug.Log($"Rules Panel visibility set to: {isRulesOpen}");
        }
        else
        {
            Debug.LogError("Rules Panel GameObject is not assigned in the Inspector!");
        }
    }

    private void HideRulesAndShowButtons()
    {
        if (rulesPanel != null) rulesPanel.SetActive(false);
        if (buttons != null) buttons.SetActive(true);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("UI Selection cleared.");
        }
    }

    void OnEnable()
    {
        if (buttonPressB != null && buttonPressB.action != null)
        {
            buttonPressB.action.performed += OnButtonPressB;
            buttonPressB.action.Enable(); // <-- CRITICAL: You must manually enable individual actions if their action map isn't globally active.
        }
    }

    void OnDisable()
    {
        if (buttonPressB != null && buttonPressB.action != null)
        {
            buttonPressB.action.performed -= OnButtonPressB;
            buttonPressB.action.Disable(); // <-- Clean up after yourself!
        }
    }

    private void OnButtonPressB(InputAction.CallbackContext callback)
    {
        // If rules are currently shown, hide them and show buttons
        if (rulesPanel != null && rulesPanel.activeSelf)
        {
            HideRulesAndShowButtons();
            Debug.Log("Button B pressed - hiding rules and showing buttons");
        }
    }
}