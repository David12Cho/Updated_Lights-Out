using UnityEngine;

public class AngryGarlic : MonoBehaviour
{
    public float followSpeed = 3f;
    private Transform playerTransform;
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject.
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing on " + gameObject.name + ". Please add one.");
        }

        // Find the player GameObject by tag and store its transform.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player GameObject is tagged 'Player'.");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 newPosition = transform.position + directionToPlayer * followSpeed * Time.fixedDeltaTime;


            rb.MovePosition(newPosition);

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                targetRotation *= Quaternion.Euler(0, 200f, 0);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * followSpeed));
            }
        }
    }
}

  






