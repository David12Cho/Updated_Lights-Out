using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    public AudioSource audioSource;
    public AudioClip[] batSqueaks;

    void Start()
    {
        offset = new Vector3(0f, hoverHeight, followDistance);
        lastPosition = transform.position;  // store initial position

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && followingBats.Count < 3)
        {
            following = true;
            followingBats.Add(this);
            Debug.Log($"Bat added! Total bats: {followingBats.Count}");
            UpdateBatPositions();

            if (batSqueaks.Length != 0)
            {
                var choice = UnityEngine.Random.Range(0, batSqueaks.Length);
                audioSource.PlayOneShot(batSqueaks[choice]);
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (followingBats.Contains(this))
        {
            followingBats.Remove(this);
            Debug.Log($"Bat removed! Remaining bats: {followingBats.Count}");
            UpdateBatPositions(); // Ensure remaining bats reposition properly
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
    }

    public static void UpdateBatPositions()
    {
        Debug.Log($"Updating bat positions. Current count: {followingBats.Count}");

        for (int i = 0; i < followingBats.Count; i++)
        {
            BatAI bat = followingBats[i];
            Vector3 basePosition = bat.player.position + bat.player.forward * -bat.followDistance + Vector3.up * bat.hoverHeight;
            float spacing = 1f;

            if (i == 1) // Second bat
                basePosition += bat.player.right * spacing;
            else if (i == 2) // Third bat
                basePosition += bat.player.right * -spacing;

            bat.transform.position = basePosition; // Force repositioning
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        followingBats.Clear();

        BatAI[] batsInScene = FindObjectsOfType<BatAI>();
        foreach (var bat in batsInScene)
        {
            Destroy(bat.gameObject);
        }
    }
}
