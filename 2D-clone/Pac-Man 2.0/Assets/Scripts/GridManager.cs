using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public LayerMask wallLayer;
    public float gridSize = 1.0f; // Adjust based on your maze layout

    private void Awake()
    {
        instance = this;
    }

    public bool IsWalkable(Vector2 position)
    {
        // Check if the given position collides with a wall
        Collider2D hit = Physics2D.OverlapCircle(position, 0.2f, wallLayer);
        return hit == null; // True if no wall is detected
    }

    public Vector2 GetClosestGridPoint(Vector2 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }
}
