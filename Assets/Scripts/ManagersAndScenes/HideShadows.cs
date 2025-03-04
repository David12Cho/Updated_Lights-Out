using UnityEngine;

public class HideShadows : MonoBehaviour
{
    void Start()
    {
        GameObject[] shadowObjects = GameObject.FindGameObjectsWithTag("Shadow");
        GameObject[] crouchObjects = GameObject.FindGameObjectsWithTag("NotCrouched");

        foreach (GameObject obj in shadowObjects)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;  // Make invisible
            }
        }

        foreach (GameObject obj in crouchObjects)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;  // Make invisible
            }
        }
    }
}
