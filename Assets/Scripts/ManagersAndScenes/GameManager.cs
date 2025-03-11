using System.Collections;
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
    public bool inShadow = false;
    private bool hasDied = false;
    // private bool finishGame = false;

    int _lives = 5;
    int _score = 0;

    public Transform player; 
    private AsyncOperation level2AsyncOperation;
    private bool isLevel2Ready = false;

    private void Start()
    {
        StartCoroutine(PreloadLevel2());
    }
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

        if (SceneManager.GetActiveScene().name == "SampleScene" && player.position.z >= -652) 
        {
            // LevelManager.Instance.LoadScene("Level 2 (Docks)", "CrossFade");
            level2AsyncOperation.allowSceneActivation = true; // now we activate the scene
            LevelManager.Instance.LoadScene("Level 2 (Docks)", "CrossFade");
            isLevel2Ready = false;
        }
    }

    private IEnumerator PreloadLevel2()
    {
        // start loading Level 2 in the background
        level2AsyncOperation = SceneManager.LoadSceneAsync("Level 2 (Docks)");
        level2AsyncOperation.allowSceneActivation = false; // don't switch to Level 2 yet

        // wait until the level is at least partially loaded (progress >= 0.9)
        while (level2AsyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        isLevel2Ready = true;
        Debug.Log("Level 2 is ready!");
    }

    public void UpdateHealth(int lives)
    {
        _lives -= lives; // Decrease on damage, increase on healing (lives is negative when healing)

        if (lives > 0) // Losing health
        {
            FindFirstObjectByType<HealthDisplay>().LoseLife();
        }
        else if (lives < 0) // Gaining health
        {
            if (_lives > 5) _lives = 5; // Ensure max lives don’t exceed 5
            FindFirstObjectByType<HealthDisplay>().GainLife(); // Update UI to add a heart
        }

        Debug.Log("Updated Lives: " + _lives);

        if (_lives <= 0)
        {
            GameOver();
        }
    }

    public int GetHealth()
    {
        return _lives;
    }

    public void UpdateShadow(bool true_false)
    {
        inShadow = true_false;
    }

    public void UpdateScore(int score)
    {
        _score += score;
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Menu");
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pause the game
        loseScreen.SetActive(true); // Show Game Over screen
    }

    public int GetLives()
    {
        return _lives;
    }

    public void RestartLevel() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}