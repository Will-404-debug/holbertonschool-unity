using UnityEngine;
using Vuforia;

public class HideOnLost : MonoBehaviour
{
    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        if (observer)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isTracked);
        }
    }
}
