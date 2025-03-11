using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShakeEffect : MonoBehaviour
{
    public RectTransform rawImage;
    public float scareDuration = 1f; // Total duration of jumpscare
    public float moveInterval = 0.1f;  // Time between position changes
    public Vector2 minPosition;        // Minimum X and Y positions
    public Vector2 maxPosition;        // Maximum X and Y positions

    private bool isScaring = false;

    void Start()
    {
        rawImage.gameObject.SetActive(false); // Hide the image initially
    }

    public void StartJumpscare()
    {
        if (!isScaring)
        {
            StartCoroutine(JumpscareRoutine());
        }
    }

    IEnumerator JumpscareRoutine()
    {
        isScaring = true;
        rawImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < scareDuration)
        {
            // Move the RawImage to a random position within the defined range
            float randomX = Random.Range(minPosition.x, maxPosition.x);
            float randomY = Random.Range(minPosition.y, maxPosition.y);
            rawImage.anchoredPosition = new Vector2(randomX, randomY);

            // Wait before changing position again
            yield return new WaitForSeconds(moveInterval);
            elapsedTime += moveInterval;
        }

        rawImage.gameObject.SetActive(false);
        isScaring = false;
    }
}

