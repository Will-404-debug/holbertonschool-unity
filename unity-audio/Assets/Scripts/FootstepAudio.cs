using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource grassLoop;
    public AudioSource rockLoop;

    private CharacterController characterController;
    private string currentSurface = "grass";

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isGrounded = characterController.isGrounded;
        bool isMoving = characterController.velocity.magnitude > 0.1f;

        if (isGrounded && isMoving)
        {
            PlayLoop(currentSurface);
        }
        else
        {
            StopLoops();
        }
    }

    void PlayLoop(string surface)
    {
        if (surface == "grass")
        {
            if (!grassLoop.isPlaying)
            {
                rockLoop.Stop();
                grassLoop.Play();
            }
        }
        else if (surface == "rock")
        {
            if (!rockLoop.isPlaying)
            {
                grassLoop.Stop();
                rockLoop.Play();
            }
        }
    }

    void StopLoops()
    {
        if (grassLoop.isPlaying) grassLoop.Stop();
        if (rockLoop.isPlaying) rockLoop.Stop();
    }

    // Called automatically when colliding with surfaces
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            string name = hit.gameObject.name.ToLower();

            if (name.Contains("grass"))
                currentSurface = "grass";
            else if (name.Contains("rock") || name.Contains("stone"))
                currentSurface = "rock";
        }
    }
}
