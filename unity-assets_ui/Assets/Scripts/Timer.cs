using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // Reference to TimerText UI element
    private float elapsedTime = 0f;
    private bool isRunning = false; // Timer starts disabled

    void Start()
    {
        this.enabled = false; // Ensure the script is disabled at start
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Count up in seconds
            UpdateTimerDisplay();
        }
    }

    // Method to start the timer (called by TimerTrigger)
    public void StartTimer()
    {
        if (!isRunning) // Prevent multiple restarts
        {
            Debug.Log("Timer Started!");
            isRunning = true;
            this.enabled = true; // Ensure Update() runs
        }
    }

    public void StopTimer()
    {
        isRunning = false; // Stop counting
        this.enabled = false; // Disable Update() from running
        Debug.Log("Timer Stopped!");
    }

    // Updates the Timer UI text
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = elapsedTime % 60;
        timerText.text = string.Format("{0}:{1:00.00}", minutes, seconds);
    }
}
