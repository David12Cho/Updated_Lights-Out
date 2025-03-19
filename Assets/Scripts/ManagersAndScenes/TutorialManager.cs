using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
        if (!hasSeenTutorial)
        {
            tutorialUI1.SetActive(true);
            hasSeenTutorial = true; // Mark tutorial as seen
        }
        else
        {
            tutorialUI1.SetActive(false);
        }
    }

    public void OnControlsButton() // when "controls" button is clicked
    {
        tutorialUI1.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }

    public void OnCloseControls() // when closing the window
    {
        tutorialUI1.SetActive(false);
        tutorialUI2.SetActive(false);

        if (audioSource && exitClickSound)
        {
            audioSource.PlayOneShot(exitClickSound);
        }
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
}
