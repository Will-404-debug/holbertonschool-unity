using UnityEngine;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    public GameObject winCanvas; // Reference to WinCanvas
    public TextMeshProUGUI finalTimeText; // Reference to FinalTime UI in WinCanvas
    public GameObject timerCanvas; // Reference to Timer Canvas
    public AudioSource victoryAudioSource; // Reference to VictoryPiano AudioSource

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 Player Reached Goal! Displaying Win Screen...");

            // Stop the background music
            LevelBGMController bgm = FindObjectOfType<LevelBGMController>();
            if (bgm != null)
            {
                bgm.StopMusic();
                Debug.Log("🎵 BGM Stopped.");
            }
            else
            {
                Debug.LogWarning("⚠️ BGM Controller not found.");
            }

            // ▶️ Play Victory Sting
            if (victoryAudioSource != null)
            {
                victoryAudioSource.Play();
                Debug.Log("🎹 Victory sting played.");
            }
            else
            {
                Debug.LogWarning("⚠️ Victory AudioSource not assigned in Inspector.");
            }

            // Display Win Canvas
            winCanvas.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            // Hide the Timer Canvas
            if (timerCanvas != null)
            {
                timerCanvas.SetActive(false);
                Debug.Log("⏳ Timer UI Hidden.");
            }
            else
            {
                Debug.LogError("❌ ERROR: Timer Canvas not assigned in Inspector!");
            }

            // Stop the Timer & Display Final Time
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.Win(finalTimeText);
            }
            else
            {
                Debug.LogError("❌ ERROR: Timer not found in the scene!");
            }
        }
    }
}
