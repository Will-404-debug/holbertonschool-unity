using UnityEngine;

public class Teleporter : MonoBehaviour 
{
	public Teleporter linkedTeleporter; // Reference to the other teleporter

	private bool isTeleporting = false; // Track if teleportation is in progress

	private float teleportCooldown = 1.0f; // Cooldown duration in seconds

	private float lastTeleportTime = -1f; // Track the last teleportation time

	private void OnTriggerEnter(Collider other)
	{
		// Check if the object entering is the Player
		if (other.CompareTag("Player") && !isTeleporting && Time.time > lastTeleportTime + teleportCooldown)
		{
			// Begin teleporting and prevent immediate re-teleport
			isTeleporting = true;
			linkedTeleporter.isTeleporting = true; // Also set the linked teleporter as teleporting

			// Move the Player to the linked Teleporter's position
			other.transform.position = linkedTeleporter.transform.position;

			// Update last teleport time and disable teleporting for the linked teleporter briefly
			lastTeleportTime = Time.time;
			linkedTeleporter.isTeleporting = true;

			// Re-enable teleporting after a short delay
			Invoke("ResetTeleport", teleportCooldown); // Using "ResetTeleport" as a string
		}
	}

	private void ResetTeleport()
	{
		isTeleporting = false;
		linkedTeleporter.isTeleporting = false; // Reset the linked teleporter's flag as well
	}
}
