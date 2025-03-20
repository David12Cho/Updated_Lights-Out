using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    // Movement and dash parameters
    public float speed;
    public float crouchSpeedFactor = 0.6f;
    public float gravityMultiplier = 1f;
    public float dashSpeed = 60f;
    public float dashDuration = 0.3f;
    private Vector3 dashDirection;
    private bool isDashing = false;
    private bool isCrouched = false;

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
    public AudioClip dashSound;

    // Ground check flag to prevent double jumps
    public GroundDetector feet;

    // for small boxes
    private bool isInNotCrouchedArea = false;

    // More Audio Clip Sounds
    public AudioClip[] damageHitSounds;
    public ShakeEffect shakeEffect;

    // for spider web
    public GameObject webOverlayUI;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _crouchAction = InputSystem.actions.FindAction("Crouch");

        _jumpAction.Enable();
        _crouchAction.Enable();

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
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
        if (_crouchAction.IsPressed() && !isCrouched)
        {
            Crouch();
        }
        else if (_crouchAction.WasReleasedThisFrame())
        {
            StandUp();
        }
    }

    void FixedUpdate()
    {
        //Implementing custom gravity for player
        _rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    private void MovePlayer()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        //Rotate
        if (moveInput.sqrMagnitude > 0.01f) // Prevents unnecessary rotation when not moving
        {
            float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            if(moveInput.y >= 0)
            {
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else 
            {
                transform.rotation = Quaternion.Euler(0, angle + 180, 0);
            }
            
        } 

        // Save last move direction for dashing
        if (moveDir != Vector3.zero)
        {
            dashDirection = moveDir;
        }

        if (!isCrouched)
        {
            _rb.linearVelocity = new Vector3(moveDir.x * speed, _rb.linearVelocity.y, moveDir.z * speed);
        }
        else
        {
            _rb.linearVelocity = new Vector3(moveDir.x * speed * crouchSpeedFactor, _rb.linearVelocity.y, moveDir.z * speed * crouchSpeedFactor);
        }

        // clamp position if on level 2
        if (SceneManager.GetActiveScene().name == "Level 2 (Docks)")
        {
            Vector3 clampedPosition = _rb.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -8.4f, 8.6f); // clamp left and right
            clampedPosition.z = Mathf.Max(clampedPosition.z, -999f); // clamp backwards
            _rb.position = clampedPosition;
        }
    }

    private void Dash()
    {
        // Ensure we have at least one bat to consume and we're not already dashing
        if (isDashing || isCrouched || BatAI.followingBats.Count <= 0) return;

        isDashing = true;

        // If no move direction, dash in the player’s current facing direction
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        Debug.Log("Dashing in direction: " + dashDirection);

        if (dashSound != null && audioSource != null)
        {   
            float volume = 0.2f;
            audioSource.PlayOneShot(dashSound, volume);
        }

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
        if (!feet || !feet.GetGrounded()) return;
        Debug.Log("Jump action triggered");
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        feet.SetGrounded(false); // Prevent further jumps until landing
    }

    private void Crouch()
    {
        // Scale down the player's y-axis to simulate crouching
        isCrouched = true;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
        Debug.Log("Crouching");
    }

    private void StandUp()
    {
        // Restore the player's original scale
        isCrouched = false;
        transform.localScale = originalScale;
        Debug.Log("Standing up");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shadow") && !isInNotCrouchedArea)
        {
            GameManager.instance.UpdateShadow(true);
            Debug.Log("In shadow!");
        } 
        else if (other.gameObject.CompareTag("NotCrouched"))
        {
            isInNotCrouchedArea = true;
            GameManager.instance.UpdateShadow(false);
            Debug.Log("Not crouched, not in shadow!");
        }
        else if (other.gameObject.CompareTag("Web")) // Web hit the player
        {
            ShowWebOverlay();
            Destroy(other.gameObject); // Remove the web projectile
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Shadow"))
        {
            GameManager.instance.UpdateShadow(false);
            Debug.Log("Exited shadow");
        }
        else if (other.gameObject.CompareTag("NotCrouched"))
        {
            isInNotCrouchedArea = false;
            if (!isInNotCrouchedArea && !feet.GetGrounded()) 
            {
                GameManager.instance.UpdateShadow(true);
                Debug.Log("Exited NotCrouched, back in shadow");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Garlic") || collision.gameObject.CompareTag("BossGarlic"))
        {
            

            if (damageHitSounds.Length != 0)
            {
                if (garlicHitSound != null)
                {
                    audioSource.PlayOneShot(garlicHitSound);
                }
            }


            if (collision.gameObject.GetComponent<AngryGarlic>() != null)
            {
                var choice = UnityEngine.Random.Range(0, damageHitSounds.Length);
                audioSource.PlayOneShot(damageHitSounds[choice]);
                shakeEffect.StartJumpscare();
            }
            
            GreenSmokeEffect smokeEffect = FindFirstObjectByType<GreenSmokeEffect>();
            if (smokeEffect != null)
            {
                smokeEffect.TriggerSmoke(transform); // Pass 'transform' instead of 'position'
            }

            Debug.Log("Collided with Garlic! (Physical Collision)");
            GameManager.instance.UpdateHealth(1);

            //Despawn Garlic
            Destroy(collision.gameObject);
        }
    }


    // SPIDER WEB

    void ShowWebOverlay()
    {
        if (webOverlayUI != null)
        {
            webOverlayUI.SetActive(true);
            StartCoroutine(HideWebOverlayAfterDelay(2f)); // Hide after 2 seconds
        }
        else
        {
            Debug.LogError("Web Overlay UI is not assigned!");
        }
    }

    IEnumerator HideWebOverlayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        webOverlayUI.SetActive(false);
    }

}