using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float turnSpeed = 10f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (movementDirection.magnitude > 0.1f)
        {
            // Rotate the player in the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}
