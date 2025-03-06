using UnityEngine;

public class BossGarlic : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float moveRadius = 2f;      // Maximum distance from the center.
    [SerializeField] private float moveSpeed = 1f;         // Speed of horizontal movement.
    private Vector3 centerPos;                             // The central position to wander around.

    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.5f;    // How high to bounce.
    [SerializeField] private float bounceSpeed = 4f;       // Speed of the vertical bounce.
    private float baseY;                                   // The starting y position.

    [Header("Z Movement Settings")]
    [SerializeField] public float chargespeed;      // Speed at which to move along the Z-axis.
    [SerializeField] public float boundary;      // Boundary to destroy

    private void Start()
    {
        // Use the starting position as the center for wandering.
        centerPos = transform.position;
        baseY = transform.position.y;
    }

    private void Update()
    {
        WanderAndBounce();

        if (transform.position.z < boundary)
        {
            Destroy(gameObject);
        }

        transform.Translate(chargespeed * Time.deltaTime * Vector3.forward);

    }

    void WanderAndBounce()
    {
        // Create horizontal wandering motion using sin and cos.
        float offsetX = Mathf.Sin(Time.time * moveSpeed) * moveRadius;
        float offsetZ = Mathf.Cos(Time.time * moveSpeed) * moveRadius;
        Vector3 wanderOffset = new Vector3(offsetX, 0, offsetZ);

        // Create a vertical bounce using an absolute sine wave so it bounces upward.
        float verticalBounce = Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;
    }

}
