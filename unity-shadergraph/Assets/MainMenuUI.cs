// MainMenuUI.cs
// Author: William Guilon Dronnier
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
