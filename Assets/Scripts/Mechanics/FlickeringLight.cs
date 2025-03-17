using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f; 
    public float smoothTime = 0.1f;   

    private Light flickerLight;
    private float targetIntensity;
    private float currentVelocity = 0f;

    void Start()
    {
        flickerLight = GetComponent<Light>();
        if (flickerLight != null)
        {
            targetIntensity = flickerLight.intensity;
        }
        else
        {
            Debug.LogWarning("No Light component found on this GameObject.");
        }
    }

    void Update()
    {
        if (flickerLight != null)
        {
            // Randomly change the target intensity
            if (Random.value < flickerSpeed)
            {
                targetIntensity = Random.Range(minIntensity, maxIntensity);
            }
            // Smoothly transition to the new intensity
            flickerLight.intensity = Mathf.SmoothDamp(flickerLight.intensity, targetIntensity, ref currentVelocity, smoothTime);
        }
    }
}

