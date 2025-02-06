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

    int _lives = 3;
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

    }

    public void UpdateHealth(int lives)
    {
        _lives -= lives;
        livesText.text = "Lives: " + _lives.ToString();
    }

    public void UpdateScore(int score)
    {
        _score += score;
        ScoreText.text = "Score: " + _score.ToString();
    }
    public void ResetTheGame()
    {
        finishGame = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

}
