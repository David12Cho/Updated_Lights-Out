using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI1;
    public GameObject tutorialUI2;
    private static bool hasSeenTutorial = false;

    public AudioSource audioSource;
    public AudioClip enterClickSound;
    public AudioClip exitClickSound;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 2 (Docks)")
        {
            hasSeenTutorial = false; // Reset for Level 2 (Docks)
        }
        
        if (!hasSeenTutorial)
        {
            hasSeenTutorial = true; // Mark tutorial as seen
            StartCoroutine(DelayedPause());
        }
        else
        {
            tutorialUI1.SetActive(false);
            Time.timeScale = 1f; // Pause the game
        }
    }

    public void OnControlsButton() // when "controls" button is clicked
    {
        tutorialUI1.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }

        Time.timeScale = 0f; // Pause the game
    }

    public void OnCloseControls() // when closing the window
    {
        tutorialUI1.SetActive(false);
        tutorialUI2.SetActive(false);

        if (audioSource && exitClickSound)
        {
            audioSource.PlayOneShot(exitClickSound);
        }

        Time.timeScale = 1f; // Pause the game
    }

    public void OnNextControls() // ">" button to switch to second window
    {
        tutorialUI1.SetActive(false);
        tutorialUI2.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }
    public void OnPreviousControls() // "<" button to switch back
    {
        tutorialUI2.SetActive(false);
        tutorialUI1.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }

    // Delay pause to wait for ccrossfade transition
    IEnumerator DelayedPause()
    {
        // Wait for 1.5 seconds before executing the following code
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 0f; // Pause the game
    }
}
