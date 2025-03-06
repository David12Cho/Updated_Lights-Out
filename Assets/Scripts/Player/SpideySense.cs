using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
        transform.Rotate(-90f, 0f, 0f, Space.Self);
    }
}
