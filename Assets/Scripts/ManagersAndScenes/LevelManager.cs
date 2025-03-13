using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI; 

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject transitionsContainer;
    public Slider progressBar;
    private SceneTransition[] transitions;
    private bool isSceneLoading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (transitionsContainer != null && transitions == null)
        {
            transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
        }
    } 

    public void LoadScene(string sceneName, string transitionName)
    {
        if (isSceneLoading) return;
        if (SceneManager.GetActiveScene().name == sceneName) return;

        isSceneLoading = true;
        Debug.Log("Attempting to load scene: " + sceneName);
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        yield return transition.AnimateTransitionIn();

        progressBar.gameObject.SetActive(true);

        while (scene.progress < 0.9f) 
        {
            Debug.Log($"Loading progress: {scene.progress}");
            progressBar.value = scene.progress;
            yield return null;
        }

        progressBar.value = 1f;

        scene.allowSceneActivation = true;

        yield return null;

        // ensure all objects are fully loaded and initialized
        // yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();

        isSceneLoading = false;
    }
}
