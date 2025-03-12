using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource;  // Assign in Inspector or get dynamically
    public AudioClip hoverSound;     // Assign a hover sound effect in Inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource && hoverSound)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
