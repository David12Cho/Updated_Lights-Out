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
    [SerializeField] private float detectionRadius = 5f; // How close the player must be
    [SerializeField] private GameObject angryGarlicPrefab; // Assign your angry garlic prefab here

    private float baseY;

    private void Start()
    {
        baseY = transform.position.y;
        Debug.Log("Garlic started at position: " + transform.position);
    }

    private void Update()
    {
        PatrolAndBounce();
        CheckPlayerDetection();
    }

    void PatrolAndBounce()
    {
        // Calculate horizontal patrol position using PingPong.
        float t = Mathf.PingPong(Time.time * patrolSpeed, 1f);
        Vector3 horizontalPos = Vector3.Lerp(targetPos1, targetPos2, t);
        // Calculate bounce offset using a sine wave.
        float bounceOffset = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;
        // Update position with horizontal movement and vertical bounce.
        transform.position = new Vector3(horizontalPos.x, baseY + bounceOffset, horizontalPos.z);
        
        Debug.Log("Updated garlic position: " + transform.position);
    }

    void CheckPlayerDetection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log("Distance to player: " + distance);
            if (distance <= detectionRadius)
            {
                Debug.Log("Player detected within range (" + distance + " <= " + detectionRadius + ").");
                //BecomeAngry();
            }
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. Ensure the player is tagged 'Player'.");
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
            if (angryGarlic != null)
            {
                Debug.Log("Angry garlic instantiated successfully: " + angryGarlic.name);
            }
            else
            {
                Debug.LogError("Failed to instantiate angry garlic.");
            }
        }
        else
        {
            Debug.LogWarning("Angry garlic prefab is not assigned in the Inspector!");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}



