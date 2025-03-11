using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject garlicPrefab;
    public GameObject obstaclePrefab;
    public float speedLimit;
    public float garlicSpawnTime;
    public float obstacleSpawnTime;

    float timeSinceGarlic = 0f;
    float timeSinceObstacle = 0f;
    float ObstacleLifetime = 10f;

    Vector3 obsSpawnPosition;

    void Update()
    {
        timeSinceGarlic += Time.deltaTime;
        if (timeSinceGarlic > garlicSpawnTime)
        {
            timeSinceGarlic = 0f;
            SpawnGarlic();

        }

        timeSinceObstacle += Time.deltaTime;
        if (timeSinceObstacle > obstacleSpawnTime)
        {
            timeSinceObstacle = 0f;
            SpawnObstacle();
        }


    }

    void SpawnGarlic()
    {
        var newGarlic = Instantiate(garlicPrefab);
        var enemyController = newGarlic.GetComponent<BossGarlic>();

        var speedchoice = UnityEngine.Random.Range(1, speedLimit); // randomly decides speed of garlic
        var spawnchoice = UnityEngine.Random.Range((float)-7.2, (float)6.5); // randomly decides where on the x axis that the garlic can spawn

        enemyController.chargespeed = speedchoice;
        newGarlic.transform.position = new Vector3(spawnchoice, 1, (float)24.9);

    }


    void SpawnObstacle()
    {
        int maxAttempts = 3; // Prevent infinite loops
        bool validSpawn = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            var randX = UnityEngine.Random.Range(-7.2f, 6.5f);
            var randZ = UnityEngine.Random.Range(6.2f, 24.3f);
            obsSpawnPosition = new Vector3(randX, 1, randZ);

            // Check if the spawn position is clear
            if (!Physics.CheckSphere(obsSpawnPosition, checkRadius))
            {
                validSpawn = true;
                break;
            }
        }

        if (validSpawn)
        {
            var newObs = Instantiate(obstaclePrefab, obsSpawnPosition, Quaternion.identity);
            Destroy(newObs, ObstacleLifetime); // Destroy after lifetime
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn position after multiple attempts.");
        }
    }


}
