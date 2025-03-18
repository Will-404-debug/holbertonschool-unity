using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load the selected level (Level01, Level02, Level03)
    public void LevelSelect(int level)
    {
        SceneManager.LoadScene("Level0" + level);
    }

    // Load the Options scene
    public void Options()
    {
        PlayerPrefs.SetString("LastScene", "MainMenu");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Options");
    }

    // Exit the game
    public void Exit()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}
