using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject controlsWindow; // parent gameobj for controls window
    public GameObject menuElements; // title + buttons
    public void OnNewGameButton() 
    {
        SceneManager.LoadScene(1);
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
}
