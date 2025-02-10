using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{
    [Header("Plane Settings")]
    [Tooltip("Length of the plane in the movement axis (Z axis).")]
    public float planeLength = 10f;  
    [Range(0f, 1f)]
    public float switchPercentage = 0.8f;

    [Header("References")]
    public Transform player;      // Drag in Player Transform
    public Camera mainCamera;     // Drag in Main Camera

    private bool is2D = false;    // Tracks if we are in 2D or 3D

    private float _threshold;

    void Start()
    {
        _threshold = planeLength * switchPercentage;
    }

    void Update()
    {
        CheckSwitchView();
    }

    private void CheckSwitchView()
    {
        if (!is2D && player.position.z >= _threshold)
        {
            SwitchTo2DView();
        }
        else if (is2D && player.position.z < _threshold)
        {
            SwitchTo3DView();
        }
    }

    private void SwitchTo2DView()
    {
        is2D = true;

        // Example: Switch camera to Orthographic
        mainCamera.orthographic = true;

        Debug.Log("Switched to 2D View!");
    }

    private void SwitchTo3DView()
    {
        is2D = false;

        // Switch camera back to Perspective
        mainCamera.orthographic = false;

        Debug.Log("Switched to 3D View!");
    }
}




