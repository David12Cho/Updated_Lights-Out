using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject controlsWindow; // parent gameobj for controls window
    public GameObject menuElements; // title + buttons
    public GameObject continueButton;
    public GameObject controlsButton;
    public RectTransform controlsButtonTransform; // control buttons' positioning
    private void Start()
    {
        // // testing purposes only
        // if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
        // {
        //     // first launch, delete PlayerPrefs for a fresh start
        //     PlayerPrefs.DeleteAll();

        //     // set the flag to avoid deleting again in the future
        //     PlayerPrefs.SetInt("HasLaunchedBefore", 1);
        //     PlayerPrefs.Save();
        // }
        
        // check if progress, else disable continue button
        if (PlayerPrefs.HasKey("HasSavedGame"))
        {
            continueButton.SetActive(true);
            AdjustButtonPositions(true);
        } else {
            continueButton.SetActive(false);
            AdjustButtonPositions(false);
        }
    }

    public void OnNewGameButton() 
    {
        // save game progress immediately on new game
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.SetInt("SavedScene", 1); // scene 1 is the first level
        PlayerPrefs.Save();

        LevelManager.Instance.LoadScene("SampleScene", "CrossFade");
    }

    public void OnContinueButton()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            int savedScene = PlayerPrefs.GetInt("SavedScene");
            SceneManager.LoadScene(savedScene);
        }
    }

    public void OnControlsButton() // when "controls" button is clicked
    {
        controlsWindow.SetActive(true);
        menuElements.SetActive(false);
    }

    public void OnCloseControls() // when closing the window
    {
        controlsWindow.SetActive(false);
        menuElements.SetActive(true);
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
            // if no "Continue", move the controls button to where continue button wouldve been
            controlsButtonTransform.anchoredPosition = new Vector2(controlsButtonTransform.anchoredPosition.x, -130);
        }
    }
}
