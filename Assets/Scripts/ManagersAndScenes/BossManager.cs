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

        // randomized values; speed & spawn 
        var speedchoice = UnityEngine.Random.Range(1, speedLimit); // randomly decides speed of garlic
        var spawnchoice = UnityEngine.Random.Range((float)-7.2, (float)6.5); // randomly decides where on the x axis that the garlic can spawn

        enemyController.chargespeed = speedchoice;
        newGarlic.transform.position = new Vector3(spawnchoice, 1, (float)24.9);

    }

    void SpawnObstacle()
    {
        var newObs = Instantiate(obstaclePrefab);

        var randX = UnityEngine.Random.Range((float)-7.2, (float)6.5); // randomly decides where obstacle can spawn
        var randZ = UnityEngine.Random.Range((float)6.2, (float)24.3); // randomly decides where obstacle can spawn

        newObs.transform.position = new Vector3(randX, 1, randZ);
        // Destroy the obstacle after obstacleLifetime seconds
        Destroy(newObs, ObstacleLifetime);

    }


}
