using UnityEngine;
using System.Collections;

public class LightFade : MonoBehaviour
{
    public Light dirLight;  // Assign your Directional Light in the Inspector
    public float fadeDuration = 0.2f; // Time to fade in and out
    public float maxIntensity = 2f;   // Peak light intensity

    private float duration = 0f;
    private bool tookDamage = false;
    public float darkInterval = 4.5f;
    public float lightInterval = 1.5f;
    private bool lightOn = false;


    void Start()
    {
        if (dirLight == null)
            dirLight = GetComponent<Light>(); // Auto-assign if missing
    }

    void Update()
    {
        duration += Time.deltaTime;

            if(lightOn)
            {
                if (!tookDamage && !GameManager.instance.inShadow)
                {
                    tookDamage = true;
                    // FindFirstObjectByType<HealthDisplay>().LoseLife();
                    GameManager.instance.UpdateHealth(1);
                    Debug.Log("Burned!");
                }
                if (duration >= lightInterval)
                {
                    StartCoroutine(ChangeIntensity(maxIntensity, 0, fadeDuration));
                    duration -= lightInterval;
                    tookDamage = false;
                    lightOn = false;
                }
            }
            else
            {
                if(duration >= darkInterval)
                {
                    StartCoroutine(ChangeIntensity(0, maxIntensity, fadeDuration));
                    duration -= darkInterval;
                    lightOn = true;
                }
            }


    }

    IEnumerator ChangeIntensity(float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            dirLight.intensity = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        dirLight.intensity = end; // Ensure it reaches the exact value
    }
}
