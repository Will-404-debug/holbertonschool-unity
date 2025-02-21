using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void MainMenu()
    {
        Time.timeScale = 1f; // Reset time before changing scene
        SceneManager.LoadScene("MainMenu");
    }

    public void Next()
    {
        Time.timeScale = 1f; // ✅ Ensure the next level is not paused

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If it's the last level, return to the Main Menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}