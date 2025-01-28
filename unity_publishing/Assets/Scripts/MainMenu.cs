using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.UI; // Required for Toggle component

public class MainMenu : MonoBehaviour
{
    public Material trapMat; // Material for the trap
    public Material goalMat; // Material for the goal
    public Toggle colorblindMode; // Toggle for colorblind mode

    // Method called when the Play button is pressed
    public void PlayMaze()
    {
        // Check if colorblind mode is enabled
        if (colorblindMode.isOn)
        {
            // Change the trap and goal colors for colorblind mode
            trapMat.color = new Color32(255, 112, 0, 255); // Orange for traps
            goalMat.color = Color.blue; // Blue for the goal
        }
        else
        {
            // Set the original colors for traps and goal
            trapMat.color = Color.red; // Red for traps
            goalMat.color = Color.green; // Green for the goal
        }

        // Load the maze scene (you should have the maze scene added to Build Settings)
        SceneManager.LoadScene("maze");
    }
}