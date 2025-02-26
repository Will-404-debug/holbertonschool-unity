using UnityEngine;

public class Pellet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.IncreaseScore(10); // Increase score when eaten
            Destroy(gameObject); // Remove pellet from scene
        }
    }
}
