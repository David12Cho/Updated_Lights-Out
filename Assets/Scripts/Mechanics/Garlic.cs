using UnityEngine;

public class Garlic : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float moveRadius = 2f;      // Maximum distance from the center.
    [SerializeField] private float moveSpeed = 1f;         // Speed of horizontal movement.
    private Vector3 centerPos;                             // The central position to wander around.

    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.5f;    // How high to bounce.
    [SerializeField] private float bounceSpeed = 4f;       // Speed of the vertical bounce.
    private float baseY;                                   // The starting y position.

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private GameObject angryGarlicPrefab;

    [Header ("Variable Motion Settings")]
    [SerializeField] private bool hasBounceOffsetValue = false;
    [SerializeField] private float bounceOffset;
    [SerializeField] private bool hasRotationOffsetValue = false;
    [SerializeField] private float rotationOffset;
    [SerializeField] private bool hasClockwiseValue = false;
    [SerializeField] private bool clockwise = true;

    private float bounceOffsetRad;
    private float rotationOffsetRad;
    private int direction;

    private void Start()
    {
        // Use the starting position as the center for wandering.
        centerPos = transform.position;
        baseY = transform.position.y;
        // Debug.Log("Garlic started at position: " + transform.position);


        //Assign bounce offset. Or Randomize
        if (hasBounceOffsetValue)
        {
            bounceOffsetRad = bounceOffset * Mathf.Deg2Rad;
        } 
        else
        {
            bounceOffsetRad = Random.Range(0, 360) * Mathf.Deg2Rad;
        } 

        //Assign rotation offset. Or Randomize
        if (hasRotationOffsetValue)
        {
            rotationOffsetRad = rotationOffset * Mathf.Deg2Rad;
        } 
        else
        {
            rotationOffsetRad = Random.Range(0, 360) * Mathf.Deg2Rad;
        } 

        //Assign direction. If none, randomize
        if (!hasClockwiseValue)
        {
            direction = Random.Range(0, 2) * 2 - 1;
        }
        else if (clockwise)
        {
            direction = 1;
        } 
        else
        {
            direction = -1;
        }
        
    }

    private void Update()
    {
        WanderAndBounce();
        CheckPlayerDetection();
    }

    void WanderAndBounce()
    {
        // Create horizontal wandering motion using sin and cos.
        float offsetX = Mathf.Sin(Time.time * moveSpeed + rotationOffsetRad) * moveRadius * direction;
        float offsetZ = Mathf.Cos(Time.time * moveSpeed + rotationOffsetRad) * moveRadius * direction;
        Vector3 wanderOffset = new Vector3(offsetX, 0, offsetZ);

        // Create a vertical bounce using an absolute sine wave so it bounces upward.
        float verticalBounce = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed + bounceOffsetRad)) * bounceHeight;

        // Update the garlic's position.
        transform.position = centerPos + wanderOffset + new Vector3(0, verticalBounce, 0);
        
        // Debug.Log("Updated garlic position: " + transform.position);
    }

    void CheckPlayerDetection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            // Debug.Log("Distance to player: " + distance);
            if (distance <= detectionRadius)
            {
                // Debug.Log("Player detected within range (" + distance + " <= " + detectionRadius + ").");
                // Uncomment the next line to trigger anger when the player is close.
                // BecomeAngry();
            }
        }
        else
        {
            // Debug.LogWarning("Player not found in the scene. Ensure the player is tagged 'Player'.");
        }
    }

    void BecomeAngry()
    {
        // Debug.Log("Garlic is becoming angry at position: " + transform.position);
        
        if (angryGarlicPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            // Debug.Log("Spawning angry garlic at: " + spawnPosition);
            
            GameObject angryGarlic = Instantiate(angryGarlicPrefab, spawnPosition, transform.rotation);
            if (angryGarlic != null)
            {
                // Debug.Log("Angry garlic instantiated successfully: " + angryGarlic.name);
            }
            else
            {
                // Debug.LogError("Failed to instantiate angry garlic.");
            }
        }
        else
        {
            // Debug.LogWarning("Angry garlic prefab is not assigned in the Inspector!");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


