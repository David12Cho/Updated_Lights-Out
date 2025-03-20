using UnityEngine;

public class LightSourceRotation : MonoBehaviour
{
    public float rotationSpeed; // Degrees per second
    public float initialOffset;

    void Start()
    {
        transform.Rotate(Vector3.up, initialOffset);
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}