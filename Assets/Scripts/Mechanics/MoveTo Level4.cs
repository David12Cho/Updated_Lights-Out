using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load (must match the Build Settings)")]
    public string nextSceneName = "Level 4 (Boss)";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the specified scene
            SceneManager.LoadScene(nextSceneName);
            Debug.Log("Loading scene: " + nextSceneName);
        }
    }
}

