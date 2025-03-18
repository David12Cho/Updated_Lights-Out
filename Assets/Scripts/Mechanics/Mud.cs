using UnityEngine;

public class Mud : MonoBehaviour
{
    public float slowDownFactor = 0.5f; // How much the player's speed will be reduced
    private float originalSpeed; // To store the player's original speed
    private PlayerController playerInMud; // Track the player in the mud zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                originalSpeed = playerMovement.speed;
                playerMovement.speed *= slowDownFactor;
                playerInMud = playerMovement; // Track the player
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null && playerInMud == playerMovement)
            {
                playerMovement.speed = originalSpeed;
                playerInMud = null; // Clear tracking when the player leaves
            }
        }
    }

    private void OnDestroy()
    {
        // Restore the player's speed if they are still in the mud when it's destroyed
        if (playerInMud != null)
        {
            playerInMud.speed = originalSpeed;
            playerInMud = null; // Clear reference
        }
    }
}
