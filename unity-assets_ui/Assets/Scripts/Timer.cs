using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to Timer UI in Timer Canvas
    private float elapsedTime = 0f;
    private bool isRunning = false; // Timer starts disabled
    private bool hasStarted = false; // Prevents multiple starts

    void Start()
    {
        this.enabled = false; // Ensure Update() is disabled at start
        if (timerText == null)
        {
            Debug.LogError("❌ ERROR: TimerText is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Count up in seconds
            UpdateTimerDisplay();
        }

        // Start the timer if the player moves
        if (!hasStarted && PlayerHasMoved())
        {
            StartTimer();
        }
    }

    // **Start the timer when the player moves**
    public void StartTimer()
    {
        if (!isRunning) // Prevent multiple restarts
        {
            Debug.Log("🎮 Timer Started!");
            isRunning = true;
            hasStarted = true;
            this.enabled = true; // Enable Update()
        }
    }

    // **Stops the Timer**
    public void StopTimer()
    {
        isRunning = false; // Stop counting
        this.enabled = false; // Disable Update()
        Debug.Log("🛑 Timer Stopped!");
    }

    // **Updates the Timer UI text**
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = GetFormattedTime();
        }
        else
        {
            Debug.LogError("❌ ERROR: TimerText UI is NOT assigned!");
        }
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = elapsedTime % 60;
        return string.Format("{0}:{1:00.00}", minutes, seconds);
    }

    // **Called when the player wins to stop the timer and update WinCanvas**
    public void Win(TextMeshProUGUI finalTimeText)
    {
        StopTimer();
        if (finalTimeText != null)
        {
            finalTimeText.text = GetFormattedTime();
            Debug.Log($"🏆 Final Time: {finalTimeText.text}");
        }
        else
        {
            Debug.LogError("❌ ERROR: FinalTime TextMeshPro not assigned in Inspector!");
        }
    }

    // **Detect Player Movement**
    private bool PlayerHasMoved()
    {
        if (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Jump"))
        {
            Debug.Log("🎮 Player moved! Timer should start now.");
            return true;
        }
        return false;
    }
}
