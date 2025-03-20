using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip sampleSceneMusic;  // Assign music for Sample Scene
    public AudioClip level2Music;       // Assign music for Level 2
    public AudioClip level3Music;
    public AudioClip level4Music;

    private AudioSource audioSource;

    private void Awake()
    {
        if (FindObjectsOfType<BackgroundMusic>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Keep music manager between scene loads

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        SceneManager.sceneLoaded += OnSceneLoaded; // Detect scene changes
    }

    private void Start()
    {
        PlayMusicForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        if (audioSource == null) return;

        AudioClip newClip = null;

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            newClip = sampleSceneMusic;
        }
        else if (SceneManager.GetActiveScene().name == "Level 2 (Docks)")
        {
            newClip = level2Music;
        } 
        else if (SceneManager.GetActiveScene().name == "Level 3 (Lighthouse)")
        {
            newClip = level3Music;
        } 
        else if (SceneManager.GetActiveScene().name == "Level 4 (Boss)")
        {
            newClip = level4Music;
        }

        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}