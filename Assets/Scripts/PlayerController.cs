using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    InputAction _moveAction;
    Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction(actionNameOrId: "Move");
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 newVelocity = new Vector3(moveInput.x, 0, moveInput.y) * speed;
        _rb.linearVelocity = newVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Garlic"))
        {
            GameManager.instance.UpdateHealth(1);
        }
    }
}
