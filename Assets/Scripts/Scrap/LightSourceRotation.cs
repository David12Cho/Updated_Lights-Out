using UnityEngine;

public class LighthouseRotation : MonoBehaviour
{
    public float rotationSpeed; // Degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}