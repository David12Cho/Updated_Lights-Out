using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI countText;
    public GameObject loseScreen;
    private bool hasDied = false;
    private bool finishGame = false;

    int _lives = 5;
    int _score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (!hasDied && _lives <= 0)
        {
            hasDied = true;
            Time.timeScale = 0f;
            loseScreen.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // if the player clicks the esc key, go back to the menu
        {
            ReturnToMenu();
        }

    }

    public void UpdateHealth(int lives)
    {
        _lives -= lives;
       // livesText.text = "Lives: " + _lives.ToString();

       Debug.Log("Lose Life: " + lives);

        if (_lives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateScore(int score)
    {
        _score += score;
      //  ScoreText.text = "Score: " + _score.ToString();
    }
    public void ResetTheGame()
    {
        finishGame = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Menu");
    }

    void GameOver()
    {
        Time.timeScale = 0f; // Pause the game
        loseScreen.SetActive(true); // Show Game Over screen
    }

}
