using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Material dissolveMaterial;
    public float dissolveSpeed = 1f;
    private float dissolveAmount = 0f;

    void Update()
    {
        // Return to normal visibility
        if (Input.GetKeyDown(KeyCode.R))
        {
            dissolveAmount = 0f;
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
        }

        // Dissolve when holding space
        if (Input.GetKey(KeyCode.Space))
        {
            if (dissolveAmount < 1f)
            {
                dissolveAmount += Time.deltaTime;
                dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            }
        }
    }
}
