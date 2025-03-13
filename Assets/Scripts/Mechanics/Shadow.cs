using UnityEngine;

public class Shadow : MonoBehaviour
{
    public delegate void ShadowDestroyedHandler(GameObject shadowObject);
    public static event ShadowDestroyedHandler OnShadowDestroyed;

    private void OnDestroy()
    {
        OnShadowDestroyed?.Invoke(gameObject); // Notify PlayerController
    }
}