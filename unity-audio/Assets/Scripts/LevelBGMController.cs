using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBGMController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the player returns to MainMenu, stop this music
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        Destroy(gameObject);
    }
}
