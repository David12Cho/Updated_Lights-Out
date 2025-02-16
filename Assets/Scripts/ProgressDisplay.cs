using UnityEngine;
using UnityEngine.UI;

public class ProgressDisplay : MonoBehaviour
{
    public Image pieImage; // Assign this in Unity
    private float totalTime = 100f; // 5 minutes
    private float elapsedTime = 0f;

    void Update()
    {
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = 1 - (elapsedTime / totalTime); // Decrease fill over time
            pieImage.fillAmount = fillAmount;
        }
        else
        {
            pieImage.fillAmount = 0f; // Timer reaches 0
            Debug.Log("Time's up!");
        }
    }
}
