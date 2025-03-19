using UnityEngine;
using Cinemachine;

public class CutsceneController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject timerCanvas;
    public Animator cutsceneAnimator;

    private PlayerController playerController;
    private bool cutsceneEnded = false;

    void Start()
    {
        // Disable PlayerController and TimerCanvas at the start
        playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        if (timerCanvas != null)
            timerCanvas.SetActive(false);    
    }

    void Update()
    {
       // Check if the cutscene animation has finished
       if (!cutsceneEnded && cutsceneAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !cutsceneAnimator.IsInTransition(0))
       {
            EndCutscene();
       }
    }

    void EndCutscene()
    {
        // Enable the Main Camera, PlayerController, and TimerCanvas
        if (mainCamera != null)
            mainCamera.SetActive(true);
        
        if (playerController != null)
            playerController.enabled = (true);
        
        if (timerCanvas != null)
            timerCanvas.SetActive(true);
        
        // Disable this script
        cutsceneEnded = true;
        this.enabled = false;
    }
}
