using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertYToggle; // UI Toggle for Inverting Y-Axis
    public Button applyButton; // Public button reference for Apply
    private string previousScene; // Store the previous scene name

    void Start()
    {
        // Load the last scene before entering options
        previousScene = PlayerPrefs.GetString("LastScene", "MainMenu");

        // Load saved invert Y setting
        if (PlayerPrefs.HasKey("InvertY"))
        {
            bool isInverted = PlayerPrefs.GetInt("InvertY") == 1;
            invertYToggle.isOn = isInverted;
        }

        // Ensure Apply button is assigned and functional
        if (applyButton != null)
        {
            applyButton.onClick.AddListener(Apply);
        }
        else
        {
            Debug.LogError("❌ ERROR: Apply Button not assigned in Inspector!");
        }

        Debug.Log($"Loaded Previous Scene: {previousScene}");
        Debug.Log($"Loaded Invert Y Setting: {invertYToggle.isOn}");
    }

    public void Apply()
    {
        Debug.Log("Apply Button Clicked!");

        // Save invert Y setting
        PlayerPrefs.SetInt("InvertY", invertYToggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); // Ensure settings are saved permanently

        Debug.Log($"Invert Y Saved: {invertYToggle.isOn}");

        // Return to the previous scene
        SceneManager.LoadScene(previousScene);
    }

    public void Back()
    {
        Debug.Log("Back Button Clicked! Returning to previous scene.");

        // Discard changes and return to previous scene
        SceneManager.LoadScene(previousScene);
    }
}
