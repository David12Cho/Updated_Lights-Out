using UnityEngine;
using System.Collections;

public class LightHouse : MonoBehaviour
{
    public Light dirLight;  // Assign your Directional Light in the Inspector
    public float fadeDuration = 0.2f; // Time to fade in and out
    public float maxIntensity = 2f;   // Peak light intensity

    private float duration = 0f;
    private float durationForSound = 0f;
    private bool tookDamage = false;
    public float darkInterval = 4.5f;
    public float lightInterval = 1.5f;
    public float timeBeforeWarn = 3.0f;
    private bool lightOn = false;

    public AudioSource audioSource;
    public AudioClip warningSound;
    public AudioClip damageSound;
    private bool playedSound = false;
    private bool immuneToLight = false;

    public SpideySense spideySense;

    public BlurEffect blur;
    
    void Start()
    {
        if (dirLight == null)
        {
            dirLight = GetComponent<Light>(); // Auto-assign if missing
        }
        immuneToLight = PlayerPrefs.GetInt("ImmuneToLight", 0) == 1;
    }

    void Update()
    {
        duration += Time.deltaTime;

            if(lightOn)
            {
                if (!tookDamage && !GameManager.instance.inShadow && !immuneToLight)
                {
                    tookDamage = true;
                    // FindFirstObjectByType<HealthDisplay>().LoseLife();
                    GameManager.instance.UpdateHealth(1);
                    audioSource.PlayOneShot(damageSound);
                    if (blur != null)
                    {
                        blur.TriggerBlur();
                    }
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
                if(!playedSound){
                    durationForSound += Time.deltaTime;
                    if(durationForSound >= timeBeforeWarn)
                    {
                        durationForSound = 0.0f;
                        audioSource.PlayOneShot(warningSound);
                        if (spideySense != null)
                        {
                            spideySense.SetActive(true);
                        }
                        playedSound = true;
                    }
                }
                

                if(duration >= darkInterval)
                {
                    StartCoroutine(ChangeIntensity(0, maxIntensity, fadeDuration));
                    duration -= darkInterval;
                    lightOn = true;
                    playedSound = false;
                    if (spideySense != null)
                    {
                        spideySense.SetActive(false);
                    }
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
