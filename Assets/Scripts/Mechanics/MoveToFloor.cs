using UnityEngine;
public class MoveToFloor : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination; // The point where the player will be teleported

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if the player collides with this object
        {
            if (teleportDestination != null)
            {
                other.transform.position = teleportDestination.position; // Teleports the player
                Debug.Log("Player teleported to: " + teleportDestination.position);
            }
            else
            {
                Debug.LogError("Teleport destination not set. Assign a Transform in the Inspector.");
            }
        }
    }
}
