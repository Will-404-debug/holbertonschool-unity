using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    
    // UI Elements
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private GameObject livesPanel;
    [SerializeField] private GameObject lifeIconPrefab;

    private bool gameOver = false;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;
    private int ghostMultiplier = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        gameOverText.enabled = false;
        readyText.enabled = false;
        gameOver = false;
        UpdateLivesDisplay();
        NewGame();
    }

    private void Update()
    {
        if (gameOver && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOver = false;
        gameOverText.enabled = false;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;
        readyText.enabled = true; // Show "READY!"

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();

        Invoke(nameof(HideReadyText), 2f); // Hide "READY!" after 2 seconds
    }

    private void HideReadyText()
    {
        Debug.Log("Hiding READY! text now."); // Debugging
        readyText.enabled = false; 
        readyText.alpha = 0f; // Force hide as backup
    }

    private void ResetState()
    {
        pacman.gameObject.SetActive(true);
        pacman.ResetState();

        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(true);
            ghost.ResetState();
        }
    }

    private void GameOver()
    {
        gameOver = true;
        gameOverText.enabled = true;
        StartCoroutine(ReturnToMainMenuAfterDelay());

        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = $"x{lives}";
        UpdateLivesDisplay();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString("D6");
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();
        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(NewRound), 3f);
        }
        else
        {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private void UpdateLivesDisplay()
    {
        foreach (Transform child in livesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < lives; i++)
        {
            Instantiate(lifeIconPrefab, livesPanel.transform);
        }
    }

    private IEnumerator FlashReadyText()
    {
        readyText.enabled = true;
        for (int i = 0; i < 6; i++)
        {
            readyText.enabled = !readyText.enabled;
            yield return new WaitForSeconds(0.3f);
        }
        readyText.enabled = true;
    }

    private IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
