using UnityEngine;

public class BossHeartPickup : MonoBehaviour
{
    private bool collected = false; // Prevents duplicate collection

    private void OnTriggerEnter(Collider other) // Changed to OnTriggerEnter
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("i have touched it");
            if (GameManager.instance.GetLives() < 5) // Only allow collecting if not full
            {
                collected = true;
                GameManager.instance.UpdateHealth(-1); // Fixed to correctly increase HP
                Destroy(gameObject); // Remove from scene
            }
        }
    }
}
