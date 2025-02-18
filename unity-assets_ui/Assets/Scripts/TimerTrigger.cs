using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if Player exits the trigger
        {
            Timer playerTimer = other.GetComponent<Timer>();

            if (playerTimer != null)
            {
                playerTimer.enabled = true; // Enable the Timer script
                playerTimer.StartTimer(); // Start counting
                Debug.Log("Timer Triggered!");
            }

            Destroy(gameObject); // Remove the trigger so it doesn’t trigger again
        }
    }
}
