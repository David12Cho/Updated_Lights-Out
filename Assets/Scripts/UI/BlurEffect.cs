using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class BlurEffect : MonoBehaviour
{
    private PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;
    private bool isBlurring = false;
    public float blurDuration = 1.5f; // Time before blur turns off

    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        
        if (postProcessVolume.profile.TryGetSettings(out depthOfField))
        {
            depthOfField.active = false; // Start with blur off
        }
        else
        {
            Debug.LogError("Depth of Field effect not found in Post-Processing Profile!");
        }
    }

    public void TriggerBlur()
    {
        if (!isBlurring)
        {
            StartCoroutine(BlurRoutine());
        }
    }

    IEnumerator BlurRoutine()
    {
        isBlurring = true;
        
        if (depthOfField != null)
        {
            depthOfField.active = true;  // Instantly enable blur
            depthOfField.focusDistance.value = 0.1f; // Strong blur effect
            Debug.Log("Blur turned ON");
        }

        yield return new WaitForSeconds(blurDuration); // Wait for duration

        if (depthOfField != null)
        {
            depthOfField.active = false; // Instantly disable blur
            Debug.Log("Blur turned OFF");
        }

        isBlurring = false;
    }
}