using UnityEngine;

public class Garlic : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Vector3 targetPos1;
    [SerializeField] private Vector3 targetPos2;
    [SerializeField] private float patrolSpeed = 1f;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.5f;
    [SerializeField] private float bounceSpeed = 4f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;    // How close the player must be
    [SerializeField] private float detectionAngle = 45f;      // Field of view angle (half cone angle)
    [SerializeField] private GameObject angryGarlicPrefab;    // Assign your angry garlic prefab here

    private float baseY;

    private void Start()
    {
        baseY = transform.position.y;
    }

    private void Update()
    {
        PatrolAndBounce();
        CheckPlayerDetection();
    }

    void PatrolAndBounce()
    {
        float t = Mathf.PingPong(Time.time * patrolSpeed, 1f);
        Vector3 horizontalPos = Vector3.Lerp(targetPos1, targetPos2, t);
        float bounceOffset = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;
        transform.position = new Vector3(horizontalPos.x, baseY + bounceOffset, horizontalPos.z);
    }

    void CheckPlayerDetection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float distance = directionToPlayer.magnitude;

            // Check if within detection radius
            if (distance <= detectionRadius)
            {
                // Normalize the direction vector for angle calculation
                directionToPlayer.Normalize();

                // Calculate the angle between garlic's forward and the direction to the player
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                // Check if the player is within the vision cone
                if (angleToPlayer <= detectionAngle)
                {
                    Debug.Log($"Player detected within {detectionRadius} units and {detectionAngle}° vision cone (angle: {angleToPlayer}).");
                    BecomeAngry();
                }
            }
        }
    }

    void BecomeAngry()
    {
        Debug.Log("Garlic is becoming angry at position: " + transform.position);

        if (angryGarlicPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Debug.Log("Spawning angry garlic at: " + spawnPosition);
            GameObject angryGarlic = Instantiate(angryGarlicPrefab, spawnPosition, transform.rotation);
            Debug.Log("Angry garlic instantiated: " + angryGarlic.name);
        }
        else
        {
            Debug.LogWarning("Angry garlic prefab is not assigned in the inspector!");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw vision cone lines (approximate)
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle, 0) * transform.forward;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRadius);
    }
}


