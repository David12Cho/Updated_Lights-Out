using UnityEngine;


public class GroundDetector : MonoBehaviour
{       

    // Ground check flag to prevent double jumps
    private bool isGrounded = true;

    private void OnTriggerEnter(Collider other)
    {
        // When colliding with an object tagged "Ground", enable jumping again.
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Table"))
        {
            isGrounded = true;
            Debug.Log("Grounded!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // When colliding with an object tagged "Ground", enable jumping again.
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Table"))
        {
            isGrounded = true;
            // Debug.Log("Grounded!");
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = false;
    //         Debug.Log("Not grounded!");
    //     }
    // }

    public bool GetGrounded()
    {
        return isGrounded;
    }

    public void SetGrounded(bool wha)
    {
        isGrounded = wha;
    }
}