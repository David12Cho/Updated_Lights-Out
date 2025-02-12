using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnNewGameButton() 
    {
        SceneManager.LoadScene(1);
    }
}
