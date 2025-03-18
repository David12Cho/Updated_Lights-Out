using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public RectTransform healthBar; // Assign your UI image's RectTransform here
    public float totalTime;  // Total duration for the decrease
    private float initialWidth;     // Stores the starting width
    public float elapsedTime = 0f; // Tracks time elapsed

    void Start()
    {
        if (healthBar != null)
        {
            initialWidth = healthBar.sizeDelta.x; // Save the initial width
        }
        else
        {
            Debug.LogError("HealthBar RectTransform not assigned.");
        }
    }

    void Update()
    {
        if (healthBar != null)
        {
            elapsedTime += Time.deltaTime;

            // Calculate new width based on elapsed time
            float newWidth = Mathf.Lerp(initialWidth, 0, elapsedTime / totalTime);

            // Ensure the width doesn't go negative
            newWidth = Mathf.Max(newWidth, 0);

            // Update the health bar size
            healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y);

            // Optional: Destroy or disable when fully depleted
            if (newWidth <= 0)
            {
                Debug.Log("Boss health bar depleted!");
                // Destroy(healthBar.gameObject); // Uncomment if you want to remove it
            }
        }
    }
}
