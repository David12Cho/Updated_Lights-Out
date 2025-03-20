using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Object Prefabs")]
    public GameObject garlicPrefab;
    public GameObject mudPrefab;
    public GameObject spiderPrefab;
    public GameObject batPrefab;
    public GameObject heartPrefab;
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Time Limits")]
    public float garlicspeedLimit;
    public float garlicSpawnTime;
    public float obstacleSpawnTime;
    public float mudSpawnTime;
    public float spiderSpawnTime;
    public float batSpawnTime;
    public float lifeSpawnTime;

    [Header("Time Sinces")]
    float timeSinceGarlic = 0f;
    float timeSinceObstacle = 0f;
    float timeSinceMud = 0f;
    float timeSinceSpider = 0f;
    float timeSinceBat = 0f;
    float timeSinceLife = 0f;

    [Header("Helper Lifetimes")]
    float BatLifetime = 5f;
    float ObstacleLifetime = 10f;

    [Header("Obstacle Conflict Prevention")]
    Vector3 obsSpawnPosition;
    public float checkRadius = 1.5f;
    bool validSpawn = true;
    public LayerMask obstacleLayer;
    private GameObject[] currentObstacles = new GameObject[2]; // Tracks two active obstacles

    [Header("Instakill Settings")]
    public float timeToInstaKill = 30f; // Time in seconds before insta-kill triggers
    private Vector3 lastPlayerPosition;
    private float idleTimer = 0f;

    [Header("General Gameplay Settings")]
    private float elapsedTime = 0f; // Keeps track of time since the level has begun
    private bool stage1CompleteCalled = false; // Flag to track Stage1Complete
    private bool stage2CompleteCalled = false;
    private bool warningShown = false; // Flag to track if warning is shown
    [SerializeField] private GameObject lightMechanic;
    [SerializeField] private GameObject stage1Overlay;
    [SerializeField] private GameObject stage2Overlay;
    [SerializeField] private GameObject killwarning;

    void Update()
    {
        CheckPlayerIdle(); // instakill checker
        elapsedTime += Time.deltaTime; // keep track of elapsed time

        if (elapsedTime < 30f)
        {
            Stage1Logic();
        } else if (elapsedTime < 60f)
        {
            if (!stage1CompleteCalled)
            {
                Stage1Complete();
                stage1CompleteCalled = true; // Mark as completed
            }

            Stage2Logic();
        } else if (elapsedTime < 90f)
        {
            if (!stage2CompleteCalled)
            {
                Stage2Complete();
                stage2CompleteCalled = true; // Mark as completed
            }

            Stage3Logic(); 
        } else if (elapsedTime < 120f)
        {
            // add.. scene change.. here... yes... ugh.. the gmae.. is done.. my name... is edwin..
        }

    }

    void Stage1Logic()
    {
        timeSinceGarlic += Time.deltaTime;
        if (timeSinceGarlic > garlicSpawnTime - 4)
        {
            timeSinceGarlic = 0f;
            SpawnGarlic();

        }

        timeSinceMud += Time.deltaTime;
        if (timeSinceMud > mudSpawnTime)
        {
            timeSinceMud = 0f;
            SpawnMud();

        }

        if (GameManager.instance.GetLives() < 3) // Only allow collecting if not full
        {
            // health return appears if health is lower than two
            timeSinceLife += Time.deltaTime;
            if (timeSinceLife > lifeSpawnTime)
            {
                timeSinceLife = 0f;
                SpawnLife();

            }
        }

    }

    void Stage2Logic()
    {

        // time since garlic gets faster
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
            SpawnObstacle2();
        }

        timeSinceMud += Time.deltaTime;
        if (timeSinceMud > mudSpawnTime)
        {
            timeSinceMud = 0f;
            SpawnMud();

        }

        if (GameManager.instance.GetLives() < 3) // Only allow collecting if not full
        {
            // health return appears if health is lower than two
            timeSinceLife += Time.deltaTime;
            if (timeSinceLife > lifeSpawnTime)
            {
                timeSinceLife = 0f;
                SpawnLife();

            }
        }

    }
    void Stage3Logic()
    {
        // time since garlic gets faster
        timeSinceGarlic += Time.deltaTime;
        if (timeSinceGarlic + 3 > garlicSpawnTime)
        {
            timeSinceGarlic = 0f;
            SpawnGarlic();

        }

        timeSinceObstacle += Time.deltaTime;
        if (timeSinceObstacle > obstacleSpawnTime)
        {
            timeSinceObstacle = 0f;
            SpawnObstacle2();
        }

        // time since mud is faster
        timeSinceMud += Time.deltaTime;
        if (timeSinceMud + 1 > mudSpawnTime)
        {
            timeSinceMud = 0f;
            SpawnMud();

        }

        // introduce spider
        timeSinceSpider += Time.deltaTime;
        if (timeSinceSpider > spiderSpawnTime)
        {
            timeSinceSpider = 0f;
            SpawnSpider();

        }

        if (GameManager.instance.GetLives() < 3) // Only allow collecting if not full
        {
            // health return appears if health is lower than two
            timeSinceLife += Time.deltaTime;
            if (timeSinceLife > lifeSpawnTime)
            {
                timeSinceLife = 0f;
                SpawnLife();

            }
        }

        /* bat ai too complicated for me to deal with sadly..

        timeSinceBat += Time.deltaTime;

        // Only spawn new bats if fewer than 3 are active
        if (BatAI.followingBats.Count < 3 && timeSinceBat > batSpawnTime)
        {
            timeSinceBat = 0f;
            SpawnBat();
        }

        */
    }

    void Stage1Complete()
    {
        if (stage1Overlay != null)
        {
            stage1Overlay.SetActive(true);
            StartCoroutine(Stage1HideOverlay(2f)); // Hide after 2 seconds
        }
        else
        {
            Debug.LogError("Web Overlay UI is not assigned!");
        }

        SpawnObstacle2(); // Force an immediate obstacle spawn
        lightMechanic.SetActive(true); // Move the lightMechanic activation here
    }
    void Stage2Complete()
    {
        // overlay textbox for dentist dialogue
        if (stage1Overlay != null)
        {
            stage2Overlay.SetActive(true);
            StartCoroutine(Stage2HideOverlay(2f)); // Hide after 2 seconds
        }

    }

    IEnumerator Stage1HideOverlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stage1Overlay.SetActive(false);
    }

    IEnumerator Stage2HideOverlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stage2Overlay.SetActive(false);
    }


    void SpawnGarlic()
    {
        var newGarlic = Instantiate(garlicPrefab);
        var enemyController = newGarlic.GetComponent<BossGarlic>();

        var speedchoice = UnityEngine.Random.Range(1, garlicspeedLimit); // randomly decides speed of garlic
        var spawnchoice = UnityEngine.Random.Range((float)-7.2, (float)6.5); // randomly decides where on the x axis that the garlic can spawn

        enemyController.chargespeed = speedchoice;
        newGarlic.transform.position = new Vector3(spawnchoice, 1, (float)24.9);

    }
    void SpawnObstacle2(){
         // Destroy current obstacles if they exist
        for (int i = 0; i < currentObstacles.Length; i++)
        {
            if (currentObstacles[i] != null)
            {
                Destroy(currentObstacles[i]);
            }
        }

        // Spawn two new obstacles
        for (int i = 0; i < 2; i++)
        {
            GameObject selectedObstacle = Instantiate(obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)]);

            var randX = UnityEngine.Random.Range(-7.2f, 6.5f);
            var randZ = UnityEngine.Random.Range(6.2f, 24.3f);

            selectedObstacle.transform.position = new Vector3(randX, 1, randZ);

            currentObstacles[i] = selectedObstacle; // Track the new obstacle
        }

        // Automatically destroy both obstacles after 10 seconds
        foreach (var obstacle in currentObstacles)
        {
            Destroy(obstacle, ObstacleLifetime);
        }

    }

    void SpawnMud()
    {
        var newMud = Instantiate(mudPrefab);

        var randX = UnityEngine.Random.Range(-7.2f, 6.5f);
        var randZ = UnityEngine.Random.Range(6.2f, 24.3f);

        newMud.transform.position = new Vector3(randX, 0.5f, randZ);

        Destroy(newMud, 5f); // Destroy after lifetime

    }
    void SpawnSpider()
    {
        var newSpider = Instantiate(spiderPrefab);

        var randX = UnityEngine.Random.Range(-7.2f, 6.5f);
        var randZ = UnityEngine.Random.Range(6.2f, 24.3f);

        newSpider.transform.position = new Vector3(randX, 1, randZ);

        Destroy(newSpider, 5f); // Destroy after lifetime

    }
    void SpawnLife()
    {
        var newLife = Instantiate(heartPrefab);

        var randX = UnityEngine.Random.Range(-7.2f, 6.5f);
        var randZ = UnityEngine.Random.Range(6.2f, 24.3f);

        newLife.transform.position = new Vector3(randX, 1, randZ);

        Destroy(newLife, 5f); // Destroy after lifetime

    }

    void SpawnBat()
    {
        // Randomized spawn position
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(-7.2f, 7.2f),
            1f,
            UnityEngine.Random.Range(6.2f, 24.3f)
        );

        GameObject newBat = Instantiate(batPrefab, randomPosition, Quaternion.identity);

        // Ensure BatAI is set up correctly
        BatAI batAI = newBat.GetComponent<BatAI>();
        if (batAI != null)
        {
            batAI.player = GameObject.FindGameObjectWithTag("Player").transform; // Assign the player reference
        }
        else
        {
            Debug.LogError("Spawned bat is missing BatAI component.");
        }

    }

    void CheckPlayerIdle()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 currentPlayerPosition = player.transform.position;

            if (currentPlayerPosition != lastPlayerPosition)
            {
                idleTimer = 0f;
                warningShown = false; // Reset warning flag if the player moves
            }
            else
            {
                idleTimer += Time.deltaTime;
            }

            if (idleTimer >= (timeToInstaKill / 2) && !warningShown)
            {
                warningShown = true; // Mark the warning as shown

                if (killwarning != null)
                {
                    killwarning.SetActive(true);
                    StartCoroutine(WarningForKill(2f));
                }
                else
                {
                    Debug.LogError("Web Overlay UI is not assigned!");
                }
            }

            // Trigger instakill if player stands still for too long
            if (idleTimer >= timeToInstaKill)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.lives = 0;
                    Debug.Log("Player stood still for too long. Instant kill activated!");
                }
                else
                {
                    Debug.LogError("GameManager not found in the scene.");
                }
            }

            lastPlayerPosition = currentPlayerPosition; // Update the last known position
        }
    }
    IEnumerator WarningForKill(float delay)
    {
        yield return new WaitForSeconds(delay);
        killwarning.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = validSpawn ? Color.green : Color.red; // Green if spawn is valid, red if invalid
        Gizmos.DrawWireSphere(obsSpawnPosition, checkRadius);
    }

}
