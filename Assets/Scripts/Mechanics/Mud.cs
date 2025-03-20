using UnityEngine;

public class Mud : MonoBehaviour
{
    public float slowDownFactor = 0.5f; // How much the player's speed will be reduced
    private float originalSpeed; // To store the player's original speed
    private bool slowApplied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !slowApplied) // Checks if the player enters the zone
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                originalSpeed = playerMovement.speed;
                playerMovement.speed *= slowDownFactor;
                playerInMud = playerMovement; // Track the player
            }

            slowApplied = true;
        }

        Debug.Log("In mud");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null && playerInMud == playerMovement)
            {
                playerMovement.speed = originalSpeed; // Restore the original speed when the player exits the zone
            }

            slowApplied = false;
        }

        Debug.Log("Out of mud");
    }
}
