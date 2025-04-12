using UnityEngine;

public class LandingSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip grassClip;
    public AudioClip rockClip;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name + ", velocity: " + collision.relativeVelocity.magnitude);

        if (collision.gameObject.CompareTag("Ground") && collision.relativeVelocity.magnitude > 2f)
        {
            string platformName = collision.gameObject.name.ToLower();

            if (platformName.Contains("grass"))
            {
                audioSource.clip = grassClip;
                audioSource.Play();
            }
            else if (platformName.Contains("rock") || platformName.Contains("stone"))
            {
                audioSource.clip = rockClip;
                audioSource.Play();
            }
        }
    }
}
