using UnityEngine;

public class LightTranslate : MonoBehaviour
{
    public GameObject player;
    public float zOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player not found.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 oldLightPosition = transform.position;  
        Vector3 newPosition = new Vector3(oldLightPosition.x, oldLightPosition.y, playerPosition.z + zOffset);
        transform.position = newPosition;
    }
}
