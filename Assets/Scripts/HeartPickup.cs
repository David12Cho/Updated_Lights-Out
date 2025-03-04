using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private bool collected = false; // Prevents duplicate collection

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
                Destroy(gameObject); // Remove from scene
            }
        }
    }
}
