using UnityEngine;

public class AngryGarlic : MonoBehaviour
{
    public float followSpeed = 3f;

    private Transform playerTransform;

    void Start()
    {
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
        if (playerTransform != null && !GameManager.instance.inShadow)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            transform.position += directionToPlayer * followSpeed * Time.deltaTime;

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                

                targetRotation *= Quaternion.Euler(0, 200f, 0);

                // Smoothly rotate the enemy towards the target rotation.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
            }
        }
    }
}




