using UnityEngine;

public class PacManController : MonoBehaviour
{
    public float speed = 4.0f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 lastValidDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W)) direction = Vector2.up;
        if (Input.GetKey(KeyCode.S)) direction = Vector2.down;
        if (Input.GetKey(KeyCode.A)) direction = Vector2.left;
        if (Input.GetKey(KeyCode.D)) direction = Vector2.right;
    }

    void Move()
    {
        if (CanMove(direction))
        {
            rb.velocity = direction * speed;
            lastValidDirection = direction;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement when hitting a wall
        }
    }

    bool CanMove(Vector2 dir)
    {
        float distance = 0.1f; // Small distance to detect walls
        RaycastHit2D[] hitResults = new RaycastHit2D[1]; // Store the results
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Walls")); // Only check Walls layer

        int hitCount = rb.Cast(dir, filter, hitResults, distance);

        return hitCount == 0; // If hitCount is 0, Pac-Man can move
    }
}
