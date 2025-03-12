using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject controlsWindow1; // parent gameobj for controls window
    public GameObject controlsWindow2;
    public GameObject menuElements; // title + buttons
    public GameObject continueButton;
    public GameObject controlsButton;
    public RectTransform controlsButtonTransform; // control buttons' positioning
    public bool resetPlayerPrefsOnStart = false;
    public TMP_InputField HaungsMode;

    // Audio Stuff for buttons
    public AudioSource audioSource;
    public AudioClip enterClickSound;
    public AudioClip exitClickSound;
    public AudioClip startGameSound;

    private void Start()
    {
        // conditionally clear PlayerPrefs based on the flag (only in the Editor or first-time flag)
        if (Application.isEditor && resetPlayerPrefsOnStart)
        {
            PlayerPrefs.DeleteAll();  // clears all PlayerPrefs
            PlayerPrefs.Save();       // changes are saved
        }

        // check if progress exists, else disable continue button
        if (PlayerPrefs.HasKey("HasSavedGame"))
        {
            continueButton.SetActive(true);
            AdjustButtonPositions(true);
        }
        else
        {
            continueButton.SetActive(false);
            AdjustButtonPositions(false);
        }
    }

    public void OnNewGameButton() 
    {
        if (HaungsMode.text == "Haungs")
        {
            PlayerPrefs.SetInt("ImmuneToLight", 1); // Save immunity cheat
        }
        else
        {
            PlayerPrefs.SetInt("ImmuneToLight", 0); // Ensure normal gameplay
        }

        // save game progress immediately on new game
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.SetInt("SavedScene", 1); // scene 1 is the first level
        PlayerPrefs.Save();

        if (audioSource && startGameSound)
        {
            audioSource.PlayOneShot(startGameSound);
        }

        LevelManager.Instance.LoadScene("Cutscene-1", "CrossFade");
    }

    public void OnContinueButton()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            if (audioSource && startGameSound)
            {
                audioSource.PlayOneShot(startGameSound);
            }
            
            int savedScene = PlayerPrefs.GetInt("SavedScene");
            SceneManager.LoadScene(savedScene);
        }
    }

    public void OnControlsButton() // when "controls" button is clicked
    {
        controlsWindow1.SetActive(true);
        menuElements.SetActive(false);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }

    public void OnCloseControls() // when closing the window
    {
        controlsWindow1.SetActive(false);
        controlsWindow2.SetActive(false);
        menuElements.SetActive(true);

        if (audioSource && exitClickSound)
        {
            audioSource.PlayOneShot(exitClickSound);
        }
    }

    public void OnNextControls() // ">" button to switch to second window
    {
        controlsWindow1.SetActive(false);
        controlsWindow2.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }
    public void OnPreviousControls() // "<" button to switch back
    {
        controlsWindow2.SetActive(false);
        controlsWindow1.SetActive(true);

        if (audioSource && enterClickSound)
        {
            audioSource.PlayOneShot(enterClickSound);
        }
    }


    private void AdjustButtonPositions(bool hasProgress)
    {
        if (hasProgress)
        {
            // if "Continue" exists, position the controls button lower
            controlsButtonTransform.anchoredPosition = new Vector2(controlsButtonTransform.anchoredPosition.x, -234);
        }
        else
        {
            // if no "Continue", move the controls button to where continue button would've been
            controlsButtonTransform.anchoredPosition = new Vector2(controlsButtonTransform.anchoredPosition.x, -130);
        }
    }
}
