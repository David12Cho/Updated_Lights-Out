using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement and dash parameters
    public float speed;
    public float dashSpeed = 60f;
    public float dashDuration = 0.3f;
    private Vector3 dashDirection;
    private bool isDashing = false;

    // Jump and crouch parameters
    public float jumpForce = 10f;         
    public float crouchScale = 0.5f;       
    private Vector3 originalScale;         

    // Input actions
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _crouchAction;

    // Component references
    Rigidbody _rb;
    private AudioSource audioSource;
    public AudioClip garlicHitSound;

    // Ground check flag to prevent double jumps
    private bool isGrounded = false;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _crouchAction = InputSystem.actions.FindAction("Crouch");

        _jumpAction.Enable();
        _crouchAction.Enable();

        _rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isDashing)
        {
            MovePlayer();
        }

        // Dash when left shift is pressed and there are bats following
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            if (BatAI.followingBats.Count > 0 && !isDashing)
            {
                Dash();
            }
        }

        // Jump if the jump action is triggered and the player is grounded
        if (_jumpAction.triggered)
        {
            Jump();
        }

        // Crouch while the crouch button is held down, otherwise stand up
        if (_crouchAction.IsPressed())
        {
            Crouch();
        }
        else
        {
            StandUp();
        }
    }

    private void MovePlayer()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Save last move direction for dashing
        if (moveDir != Vector3.zero)
        {
            dashDirection = moveDir;
        }

        // Preserve vertical velocity by only setting horizontal components
        Vector3 currentVelocity = _rb.linearVelocity;
        _rb.linearVelocity = new Vector3(moveDir.x * speed, currentVelocity.y, moveDir.z * speed);
    }

    private void Dash()
    {
        // Ensure we have at least one bat to consume and we're not already dashing
        if (isDashing || BatAI.followingBats.Count <= 0) return;

        isDashing = true;

        // If no move direction, dash in the player’s current facing direction
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        Debug.Log("Dashing in direction: " + dashDirection);

        // Apply an instantaneous force in the dash direction
        _rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        // Remove one bat as a resource cost for dashing
        if (BatAI.followingBats.Count > 0)
        {
            BatAI batToRemove = BatAI.followingBats[0];
            BatAI.followingBats.RemoveAt(0);
            BatAI.UpdateBatPositions();
            Destroy(batToRemove.gameObject);
        }

        // End the dash after dashDuration seconds
        Invoke(nameof(EndDash), dashDuration);
    }

    private void EndDash()
    {
        isDashing = false;
        _rb.linearVelocity = Vector3.zero;
    }

    private void Jump()
    {
        // Only allow a jump if the player is on the ground
        if (!isGrounded) return;
        Debug.Log("Jump action triggered");
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Prevent further jumps until landing
    }

    private void Crouch()
    {
        // Scale down the player's y-axis to simulate crouching
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
        Debug.Log("Crouching");
    }

    private void StandUp()
    {
        // Restore the player's original scale
        transform.localScale = originalScale;
        Debug.Log("Standing up");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Garlic"))
        {
            GameManager.instance.UpdateHealth(1);
        }
        if (other.gameObject.CompareTag("Shadow"))
        {
            GameManager.instance.UpdateShadow(true);
            Debug.Log("In shadow!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Shadow"))
        {
            GameManager.instance.UpdateShadow(false);
            Debug.Log("Exited shadow");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When colliding with an object tagged "Ground", enable jumping again.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Garlic"))
        {
            if (garlicHitSound != null)
            {
                audioSource.PlayOneShot(garlicHitSound);
            }
            Debug.Log("Collided with Garlic! (Physical Collision)");
            GameManager.instance.UpdateHealth(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}











