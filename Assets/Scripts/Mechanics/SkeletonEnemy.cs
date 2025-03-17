using UnityEngine;
using UnityEngine.AI;

public class SkeletonEnemy : MonoBehaviour
{
    [Tooltip("Assign the player GameObject (should have the 'Player' tag)")]
    public Transform player;

    [Tooltip("Movement speed for the enemy")]
    public float moveSpeed = 3f;

    // Components
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        // Try to get the NavMeshAgent component if it exists on this prefab.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // If a NavMeshAgent is attached, use it to follow the player.
        if (agent != null)
        {
            agent.SetDestination(player.position);

            // Update the animator's "Speed" parameter based on the agent's velocity magnitude.
            if (animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
        else
        {
            // Fallback: simple movement towards the player.
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate to face the player (ignoring height differences).
            Vector3 lookTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookTarget);

            // Update the animator with a constant speed value.
            if (animator != null)
            {
                animator.SetFloat("Speed", moveSpeed);
            }
        }
    }

    // Detect collision with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // When touching the player, lose a heart.
            if (GameManager.instance != null)
            {
                GameManager.instance.UpdateHealth(1);
            }

            // Optionally, destroy the skeleton enemy after it hits the player.
            Destroy(gameObject);
        }
    }
}
