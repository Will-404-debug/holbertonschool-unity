using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game (only works in a built application)
        Debug.Log("Game is exiting..."); // This appears in Unity Editor but does nothing in a build
    }
}
