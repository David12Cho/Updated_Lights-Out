using UnityEngine;

public class BossGarlic : MonoBehaviour
{

    [Header("Z Movement Settings")]
    [SerializeField] public float chargespeed;      // Speed at which to move along the Z-axis.
    [SerializeField] public float boundary;      // Boundary to destroy

    private void Update()
    {

        if (transform.position.z < boundary)
        {
            Destroy(gameObject);
        }

        transform.Translate(chargespeed * Time.deltaTime * Vector3.forward);

    }

}
