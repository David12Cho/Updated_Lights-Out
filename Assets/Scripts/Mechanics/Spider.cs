using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private GameObject webPrefab;
    [SerializeField] private float sprayCooldown = 2f; // Time in seconds between web sprays

    private float timeSinceLastSpray = 0f;

    void Update()
    {
        timeSinceLastSpray += Time.deltaTime;
        CheckPlayerDetection();
    }

    void CheckPlayerDetection()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRadius && timeSinceLastSpray >= sprayCooldown)
            {
                SprayPlayer();
                timeSinceLastSpray = 0f; // Reset cooldown timer
            }
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. Ensure the player is tagged 'Player'.");
        }
    }

    void SprayPlayer()
    {
        if (webPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Instantiate(webPrefab, spawnPosition, transform.rotation);
        }
    }
}
