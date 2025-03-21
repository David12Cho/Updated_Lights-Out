using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private bool collected = false; // Prevents duplicate collection

    public AudioSource audioSource;
    public AudioClip soundEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.GetLives() < 5) // Only allow collecting if not full
            {
                collected = true;
                GameManager.instance.UpdateHealth(-1); // Gain a life
                FindObjectOfType<HeartManager>().RemoveHeart(this); // Notify HeartManager
                Debug.Log("Got health");

                AudioSource.PlayClipAtPoint(soundEffect, transform.position, 5.0f);
                Destroy(gameObject); // Remove from scene
            }
        }
    }
}
