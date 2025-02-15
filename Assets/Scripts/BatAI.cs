using UnityEngine;
using System.Collections.Generic;

public class BatAI : MonoBehaviour
{
    public static List<BatAI> followingBats = new List<BatAI>();
    public Transform player;  
    public float hoverHeight = 1f;  
    public float followDistance = 1.5f; 
    public float smoothSpeed = 10f;  
    public float rotationSpeed = 200f;  

    private bool following = false;  
    private Vector3 offset;  
    private Vector3 lastPosition;  

    void Start()
    {
        offset = new Vector3(0f, hoverHeight, followDistance);
        lastPosition = transform.position;  // store initial position
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && followingBats.Count < 3)
        {
            following = true;
            followingBats.Add(this);
            UpdateBatPositions();
        }
    }

    void Update()
    {
        // ff the bat isn't following, don't move
        if (!following || player == null)
        {
            return;
        }

        int index = followingBats.IndexOf(this);

        // Vector3 desiredPosition = player.position + player.forward * -followDistance + Vector3.up * hoverHeight;
        Vector3 basePosition = player.position + player.forward * -followDistance + Vector3.up * hoverHeight;
        float spacing = 1f;

        if (index == 1) // Second bat
            basePosition += player.right * spacing;
        else if (index == 2) // Third bat
            basePosition += player.right * -spacing;

        transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime * smoothSpeed);

        // Rotate the bat to face the player
        Vector3 directionToPlayer = player.position - transform.position; 
        directionToPlayer.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // transform.position = Vector3.Lerp(lastPosition, desiredPosition, Time.deltaTime * smoothSpeed);

        // // rotate the bat to face the player
        // Vector3 directionToPlayer = player.position - transform.position; 
        // directionToPlayer.y = 0f; 

        // // calc the target rotation to face the player
        // Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // // smoothly rotate the bat to the target rotation 
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // // update the last position for the next frame
        // lastPosition = transform.position;
    }

    public static void UpdateBatPositions()
    {
        for (int i = 0; i < followingBats.Count; i++)
        {
            BatAI bat = followingBats[i];
            bat.Update(); // Ensure all bats update their positions
        }
    }
}
