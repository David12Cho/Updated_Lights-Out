using UnityEngine;

public class SpideySense : MonoBehaviour
{
    public float fadeDuration = 1f; // Time to fade in/out
    private Material material; // Reference to the material
    private Color originalColor; // Store the original color
    
    void Start()
    {
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        transform.Rotate(90f, 180f, 0f, Space.Self);
    }

    public void SetActive(bool turnOn)
    {
        gameObject.SetActive(turnOn);
    }
}
