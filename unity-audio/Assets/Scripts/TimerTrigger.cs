using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("✅ Player exited the trigger - Starting Timer!");
            Timer timer = FindObjectOfType<Timer>();

            if (timer != null)
            {
                timer.StartTimer();
                hasTriggered = true; // Ensure it only triggers once
            }
            else
            {
                Debug.LogError("❌ ERROR: No Timer found in the scene!");
            }
        }
    }
}
