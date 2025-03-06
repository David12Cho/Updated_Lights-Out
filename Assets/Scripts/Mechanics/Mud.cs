using UnityEngine;

public class Mud : MonoBehaviour
{
    public float slowDownFactor = 0.5f; // How much the player's speed will be reduced
    private float originalSpeed; // To store the player's original speed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if the player enters the zone
        {
            // Assuming the player has a script with a speed variable
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                originalSpeed = playerMovement.speed; // Save the original speed
                playerMovement.speed *= slowDownFactor; // Slow down the player
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if the player exits the zone
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.speed = originalSpeed; // Restore the original speed when the player exits the zone
            }
        }
    }
}
