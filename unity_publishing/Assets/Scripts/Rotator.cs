using UnityEngine;

public class Rotator : MonoBehaviour 
{

	void Update () 
	{
		// Rotate the Coin by 45 degrees on the x-axis over time
		transform.Rotate(45 * Time.deltaTime, 0, 0);
	}
}
