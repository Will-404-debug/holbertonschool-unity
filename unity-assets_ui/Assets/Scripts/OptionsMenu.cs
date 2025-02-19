using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    // Return to the last visited scene
    public void Back()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", "MainMenu"); // Default to MainMenu if no previous scene is saved
        SceneManager.LoadScene(lastScene);
    }
}
