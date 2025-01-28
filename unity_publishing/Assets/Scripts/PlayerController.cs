using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes
using UnityEngine.UI; // Required to manipulate UI Text and UI Image
using System.Collections; // Required for coroutines

public class PlayerController : MonoBehaviour 
{
    public float speed = 5.0f; // Speed variable, can be modified in the Inspector

    private Rigidbody rb;

    private int score = 0; // Private score variable initialized to 0
    public int health = 5; // Public health variable initialized to 5

    public Text scoreText; // Public Text variable to link with the ScoreText GameObject in the Inspector
    public Text healthText; // Public Text variable to link with the HealthText GameObject in the Inspector
    public Text winLoseText; // Public Text variable to link with the WinLoseText GameObject
    public Image winLoseBG; // Public Image variable to link with the WinLoseBG GameObject

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Player
        score = 0; // Reset score to 0
        health = 5; // Reset health to 5
        SetScoreText(); // Call SetScoreText to initialize the score display
        SetHealthText(); // Call SetHealthText to initialize the health display

        // Initially hide the WinLoseText and WinLoseBG
        winLoseText.gameObject.SetActive(false);
        winLoseBG.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        // Get input from WASD or Arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        // Move the Player
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        // Check if health is 0
        if (health <= 0)
        {
            // Display "Game Over!" message
            winLoseText.text = "Game Over!";
            winLoseText.color = Color.white; // Change the WinLoseText color to white
            winLoseBG.color = Color.red; // Change the WinLoseBG color to red
            
            // Show the WinLoseText and WinLoseBG
            winLoseText.gameObject.SetActive(true);
            winLoseBG.gameObject.SetActive(true);

            // Start the coroutine to wait for 3 seconds before reloading the scene
            StartCoroutine(LoadScene(3)); // Wait for 3 seconds before reloading
        }

        // Check if the Esc key is pressed to go back to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the menu scene
            SceneManager.LoadScene("menu"); // Ensure that "menu" is the name of your menu scene
        }
    }

    // This function is called when another collider enters the trigger collider attached to this object
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup")) // Check if the other object has the tag "Pickup"
        {
            score++; // Increment the score
            SetScoreText(); // Update the score display
            other.gameObject.SetActive(false); // Disable the Coin GameObject
        }
        else if (other.CompareTag("Trap")) // Check if the other object has the tag "Trap"
        {
            health--; // Decrement the health
            SetHealthText(); // Update the health display

            // Optionally, you can add logic to handle what happens when health reaches zero
            if (health <= 0)
            {
                // The "Game Over!" display and scene reloading is handled in Update
            }
        }
        else if (other.CompareTag("Goal")) // Check if the other object has the tag "Goal"
        {
            // Display the "You Win!" message
            winLoseText.text = "You Win!";
            winLoseText.color = Color.black; // Change the WinLoseText color to black
            winLoseBG.color = Color.green; // Change the WinLoseBG color to green
            
            // Make the text and background visible
            winLoseText.gameObject.SetActive(true);
            winLoseBG.gameObject.SetActive(true);

            // Start the coroutine to wait for 3 seconds before reloading the scene
            StartCoroutine(LoadScene(3)); // Wait for 3 seconds before reloading
        }
    }

    // Method to update the score text in the UI
    void SetScoreText()
    {
        scoreText.text = "Score: " + score; // Update the ScoreText UI element with the current score
    }

    // Method to update the health text in the UI
    void SetHealthText()
    {
        healthText.text = "Health: " + health; // Update the HealthText UI element with the current health
    }

    // Coroutine that waits for the specified number of seconds before reloading the scene
    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Wait for the specified time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
