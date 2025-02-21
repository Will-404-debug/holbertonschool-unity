using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to Pause Menu UI Canvas
    public Button resumeButton, restartButton, menuButton, optionsButton; // UI Buttons
    public static bool isPaused = false; // Track if the game is paused

    void Start()
    {
        // Ensure buttons work
        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (restartButton != null) restartButton.onClick.AddListener(Restart);
        if (menuButton != null) menuButton.onClick.AddListener(LoadMainMenu);
        if (optionsButton != null) optionsButton.onClick.AddListener(LoadOptionsMenu);

        pauseMenuUI.SetActive(false); // Hide menu at start
    }

    void Update()
    {
        // Press "Escape" to pause/unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else Resume();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show Pause Menu
        Time.timeScale = 0f; // Freeze game time
        isPaused = true;
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject); // Select Resume button for keyboard/controller users
    }

    public void Options()
    {
        // Save the current scene before switching to options
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Options");
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Reset time before restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current level
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time before changing scene
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
    }

    public void LoadOptionsMenu()
    {
        Time.timeScale = 1f; // Reset time before changing scene
        SceneManager.LoadScene("Options"); // Load the Options scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quit the game (works in a build)
    }
}
