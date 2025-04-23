using UnityEngine;
using Vuforia;

public class HideOnLost : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        if (observer)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        
        // Get the Animator from this GameObject or its children
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isTracked);
        }

        // Play animation only when tracked
        if (isTracked && animator != null)
        {
            animator.SetTrigger("Show");
        }
    }
}
