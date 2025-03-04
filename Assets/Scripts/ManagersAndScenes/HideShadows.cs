using UnityEngine;

public class HideShadows : MonoBehaviour
{
    void Start()
    {
        GameObject[] shadowObjects = GameObject.FindGameObjectsWithTag("Shadow");

        foreach (GameObject obj in shadowObjects)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;  // Make invisible
            }
        }
    }
}
