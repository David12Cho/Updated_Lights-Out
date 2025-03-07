using UnityEngine;

public class GreenSmokeEffect : MonoBehaviour
{
    public GameObject greenSmokePrefab; // Assign the smoke prefab in the Inspector
    public float smokeDuration = 3f; // How long the smoke should stay

    public void TriggerSmoke(Transform player)
    {
        GameObject smoke = Instantiate(greenSmokePrefab, player.position, Quaternion.identity);
        smoke.transform.SetParent(player); // Attach smoke to the player
        Destroy(smoke, smokeDuration); // Destroy after smokeDuration
    }
}