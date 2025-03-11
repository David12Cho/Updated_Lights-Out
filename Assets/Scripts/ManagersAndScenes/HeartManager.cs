using UnityEngine;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    public List<HeartPickup> hearts = new List<HeartPickup>(); // List of all heart objects
    private Transform player;
    private HeartPickup activeHeart; // The currently active heart

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FindAllHearts();
        UpdateHeartVisibility();
    }

    void Update()
    {
        UpdateHeartVisibility();
    }

    void FindAllHearts()
    {
        hearts.Clear();
        HeartPickup[] allHearts = FindObjectsOfType<HeartPickup>(); 
        foreach (HeartPickup heart in allHearts)
        {
            hearts.Add(heart);
            heart.gameObject.SetActive(false); // Hide all hearts initially
        }
    }

    void UpdateHeartVisibility()
    {
        if (GameManager.instance.GetLives() > 3) // No heart should be visible if lives > 3
        {
            if (activeHeart != null) activeHeart.gameObject.SetActive(false);
            activeHeart = null;
            return;
        }

        HeartPickup closestHeart = null;
        float closestDistance = Mathf.Infinity;

        foreach (HeartPickup heart in hearts)
        {
            Vector3 toHeart = heart.transform.position - player.position;
            float distance = toHeart.magnitude;

            // Ensure the heart is in FRONT of the player
            if (Vector3.Dot(player.forward, toHeart) > 0) 
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHeart = heart;
                }
            }
        }

        // Update active heart
        if (closestHeart != activeHeart)
        {
            if (activeHeart != null) activeHeart.gameObject.SetActive(false);
            activeHeart = closestHeart;
            if (activeHeart != null) activeHeart.gameObject.SetActive(true);
        }
    }

    public void RemoveHeart(HeartPickup collectedHeart)
    {
        hearts.Remove(collectedHeart);
        UpdateHeartVisibility();
    }
}
