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
        }
    }

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    } 

    public void LoadScene(string sceneName, string transitionName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.Log("Scene is already loaded, skipping load: " + sceneName);
            return;  
        }
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
            progressBar.value = scene.progress;
            yield return null;
        }

        progressBar.value = 1f;

        scene.allowSceneActivation = true;

        yield return null;

        // ensure all objects are fully loaded and initialized
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        yield return new WaitForSeconds(4f); 

        progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();
    }


}
