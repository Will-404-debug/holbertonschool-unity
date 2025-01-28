using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public GameObject player; // Public variable to assign the Player GameObject in the Inspector
	private Vector3 offset; // Offset between the camera and the player

	void Start () 
	{
		// Calculate the initial offset based on the camera's initial position relative to the player
		if (player != null)
		{
			offset = transform.position - player.transform.position;
		}
	}
	
	void LateUpdate () 
	{
		// Only update the camera's position if the player is assigned
		if (player != null)
		{
			// Set the camera's position based on the player's position and the offset
			transform.position = player.transform.position + offset;
		}
	}
}
