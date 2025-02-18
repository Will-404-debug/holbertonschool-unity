using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public Timer playerTimer; // Reference to the Timer script
    public Text timerText;    // Reference to the Timer UI Text

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers this
        {
            if (playerTimer != null)
            {
                playerTimer.StopTimer(); // Stop the timer
            }

            if (timerText != null)
            {
                timerText.fontSize = 60; // Increase font size
                timerText.color = Color.green; // Change color to green
            }

            Debug.Log("Player has reached the finish line!");
        }
    }
}
