using UnityEngine;

public class BatAI : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            following = true;
        }
    }

    void Update()
    {
        // ff the bat isn't following, don't move
        if (!following)
        {
            return;
        }

        Vector3 desiredPosition = player.position + player.forward * -followDistance + Vector3.up * hoverHeight;

        transform.position = Vector3.Lerp(lastPosition, desiredPosition, Time.deltaTime * smoothSpeed);

        // rotate the bat to face the player
        Vector3 directionToPlayer = player.position - transform.position; 
        directionToPlayer.y = 0f; 

        // calc the target rotation to face the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // smoothly rotate the bat to the target rotation 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // update the last position for the next frame
        lastPosition = transform.position;
    }
}
