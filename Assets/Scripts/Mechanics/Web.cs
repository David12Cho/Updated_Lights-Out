using System.Collections;
using UnityEngine;

public class Web : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 playerPos;
    //public GameObject hitFlashImage;
    public GameObject webOverlayUI;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform.position; // Store the player's position at spawn
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player GameObject is tagged 'Player'.");
        }
    }

    void Update()
    {
        // Move towards the stored target position
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);

        // Destroy the Web when it reaches the target position
        if (Vector3.Distance(transform.position, playerPos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

}
