using UnityEngine;
using UnityEngine.EventSystems;

public class PlayHoverSFX : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && !hoverSound.isPlaying)
        {
            hoverSound.Play();
        }
    }
}
