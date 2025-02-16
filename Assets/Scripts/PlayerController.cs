using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    InputAction _moveAction;
    Rigidbody _rb;
    private Vector3 dashDirection; 
    public float dashSpeed = 60f;
    public float dashDuration = 0.3f;
    private bool isDashing = false;

    void Start()
    {
        _moveAction = InputSystem.actions.FindAction(actionNameOrId: "Move");
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDashing)
        {
            MovePlayer();
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) // the player dashes with left shift key
        {
            if (BatAI.followingBats.Count > 0 && !isDashing){
                Dash();
            }
        }
    }

    private void MovePlayer()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (moveDir != Vector3.zero)
        {
            dashDirection = moveDir; 
        }

        _rb.linearVelocity = moveDir * speed;
    }

    private void Dash()
    {
        if (isDashing || BatAI.followingBats.Count <= 0) return; // prevent dashing if no bats

        isDashing = true;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward; // dash in player's facing direction
        }

        Debug.Log("Dashing in direction: " + dashDirection);

        _rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse); // apply force instantly for dash

        if (BatAI.followingBats.Count > 0)
        {
            // reference to the bat being removed
            BatAI batToRemove = BatAI.followingBats[0];
            BatAI.followingBats.RemoveAt(0);
            BatAI.UpdateBatPositions();
            Destroy(batToRemove.gameObject);
        }

        Invoke(nameof(EndDash), dashDuration);
    }

    private void EndDash()
    {
        isDashing = false;
        _rb.linearVelocity = Vector3.zero; // stop dash
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Garlic"))
        {
            GameManager.instance.UpdateHealth(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Garlic"))
        {
            Debug.Log("Collided with Garlic! (Physical Collision)");
            FindObjectOfType<HealthDisplay>().LoseLife();
        }
    }
}
