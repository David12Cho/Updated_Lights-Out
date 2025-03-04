using UnityEngine;
using UnityEngine.UI;

public class ProgressDisplay : MonoBehaviour
{
    public Image pieImage; // Assign this in Unity
    public float totalTime = 120f; // 300f is 5 minutes
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
            // Debug.Log("Time's up!");
            GameManager.instance.GameOver();
        }
    }
}
