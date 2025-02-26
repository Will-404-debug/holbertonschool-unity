using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    public Transform pacMan;
    public float speed = 3.0f;
    private Vector2 currentDirection;
    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        transform.position = GridManager.instance.GetClosestGridPoint(transform.position);
        ChooseNewDirection();
    }

    void Update()
    {
        if (!isMoving)
        {
            Move();
        }
    }

    void Move()
    {
        isMoving = true;
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        ChooseNewDirection();
    }

    void ChooseNewDirection()
    {
        List<Vector2> possibleDirections = new List<Vector2>();

        Vector2 up = (Vector2)transform.position + Vector2.up;
        Vector2 down = (Vector2)transform.position + Vector2.down;
        Vector2 left = (Vector2)transform.position + Vector2.left;
        Vector2 right = (Vector2)transform.position + Vector2.right;

        if (GridManager.instance.IsWalkable(up)) possibleDirections.Add(Vector2.up);
        if (GridManager.instance.IsWalkable(down)) possibleDirections.Add(Vector2.down);
        if (GridManager.instance.IsWalkable(left)) possibleDirections.Add(Vector2.left);
        if (GridManager.instance.IsWalkable(right)) possibleDirections.Add(Vector2.right);

        if (possibleDirections.Count > 0)
        {
            // Move towards Pac-Man instead of random movement
            Vector2 bestDirection = possibleDirections[0];
            float shortestDistance = Vector2.Distance((Vector2)transform.position + bestDirection, pacMan.position);

            foreach (Vector2 dir in possibleDirections)
            {
                float distance = Vector2.Distance((Vector2)transform.position + dir, pacMan.position);
                if (distance < shortestDistance)
                {
                    bestDirection = dir;
                    shortestDistance = distance;
                }
            }

            currentDirection = bestDirection;
            targetPosition = GridManager.instance.GetClosestGridPoint((Vector2)transform.position + currentDirection);
        }
    }
}
