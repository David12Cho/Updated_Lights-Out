using UnityEngine;

public class AngryGarlic : MonoBehaviour
{
    // Speed at which the enemy follows the player.
    public float followSpeed = 3f;

    private Transform playerTransform;

    void Start()
    {
        // Look for the player in the scene by tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player GameObject is tagged 'Player'.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction from enemy to player.
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            transform.position += direction * followSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);
            }
        }
    }
}



