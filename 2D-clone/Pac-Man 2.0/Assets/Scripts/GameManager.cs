using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private TextMeshProUGUI readyText; // NEW: "READY!" text
    [SerializeField] private GameObject livesPanel;  // Parent object for lives
    [SerializeField] private GameObject lifeIconPrefab;  // Pac-Man life icon prefab

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
        readyText.enabled = false; // NEW: Hide "READY!" at start
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverText.enabled = false;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;
        readyText.enabled = true; // NEW: Show "READY!" when round starts

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();

        // Hide "READY!" after 2 seconds and start the game
        Invoke(nameof(HideReadyText), 2f);
    }

    private void HideReadyText()
    {
        readyText.enabled = false;
    }

    private void ResetState()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        StartCoroutine(FadeGameOverText());

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

    private IEnumerator FadeGameOverText()
    {
        gameOverText.enabled = true;
        gameOverText.text = "GAME OVER!";
        gameOverText.alpha = 0f;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            gameOverText.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
    }
}
