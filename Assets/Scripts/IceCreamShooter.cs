using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class IceCreamShooter : MonoBehaviour
{
    
    public LogicScript logic;
    /* Variable for calculating player score */
    public int playerScore;
    public Text scoreText;

    private Scene activeScene;

    /* Variables for spawn and speed for flying objects */
    public float iceCreamSpeed = 5f;
    public float iceCreamCooldown = 0.5f;
    public GameObject slowDownSpeed;
    public GameObject slowDownIceCream;


    /* SFX Variables */
    public AudioClip iceCreamSpawn;

    /* Variables for ice cream models */
    public GameObject iceCream_One;
    public GameObject iceCream_Two;
    public GameObject iceCream_Three;
    public GameObject iceCream_Four;
    public GameObject iceCream_Five;
    public GameObject iceCream_Six;
    

    /* Count destroyed (when passing border) */
    public int iceCreamDestroyed;

    /* Ice creams per level */
    int iceCreamAmount;

    /* Cones for spawning Ice Cream */
    public List<GameObject> iceCones;

    /* Load model for Ice Cream depening on level */
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level One")
        {
            iceCreamAmount = 75;
        }
        if (scene.name == "Level Two")
        {
            iceCreamAmount = 300;
            int levelOneScore = PlayerPrefs.GetInt("LevelOneScore", 0);
            playerScore += levelOneScore;
            scoreText.text = playerScore.ToString();
        }
        if (scene.name == "Level Three")
        {
            iceCreamAmount = 500;
            int levelTwoScore = PlayerPrefs.GetInt("LevelTwoScore", 0);
            playerScore += levelTwoScore;
            scoreText.text = playerScore.ToString();
        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level One")
        {
            iceCreamAmount = 75;
        }
        else if (SceneManager.GetActiveScene().name == "Level Two")
        {
            iceCreamAmount = 300;
            int levelOneScore = PlayerPrefs.GetInt("LevelOneScore", 0);
            playerScore += levelOneScore;
            scoreText.text = playerScore.ToString();
        }
        else if (SceneManager.GetActiveScene().name == "Level Three")
        {
            iceCreamAmount = 500;
            int levelTwoScore = PlayerPrefs.GetInt("LevelTwoScore", 0);
            playerScore += levelTwoScore;
            scoreText.text = playerScore.ToString();
        }
        activeScene = SceneManager.GetActiveScene();
        Cursor.visible = false; // Hide the cursor
        StartCoroutine(LoadNextSceneWithDelay());
        StartCoroutine(ShootIceCream());
        
    }
    private IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        
    }
    IEnumerator ShootIceCream()
    {   
        /* Initialize SFX */
        AudioSource iceCreamSpawn = GetComponent<AudioSource>();
        /* Used to check if spawned objects overlap */
        HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();
        while (true)
        {
            /* Change Ice Cream to level specific color */
            GameObject iceCreamPrefab;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level Two":
                    iceCreamPrefab = iceCream_Two;
                    break;
                case "Level Three":
                    iceCreamPrefab = iceCream_Four;
                    break;
                default:
                    iceCreamPrefab = iceCream_One;
                    break;
            }

            /* Shoot from 3 cones at the same time */
            var randomIndexes = Enumerable.Range(0, iceCones.Count).OrderBy(x => Random.value).Take(3);

            /* Shoot from randomly selected cones */
            spawnedPositions.Clear();
            foreach (int coneIndex in randomIndexes)
            {
                iceCreamSpawn.Play();
                Vector3 spawnPosition = iceCones[coneIndex].transform.position;
                while (spawnedPositions.Contains(spawnPosition))
                {
                    spawnPosition += Vector3.down * 1f; // Offset the position slightly
                }
                spawnedPositions.Add(spawnPosition);

                GameObject iceCream = Instantiate(iceCreamPrefab, spawnPosition, Quaternion.identity);
                Rigidbody2D iceCreamRigidbody = iceCream.GetComponent<Rigidbody2D>();
                iceCreamRigidbody.AddForce(Vector2.up * iceCreamSpeed, ForceMode2D.Impulse);
            }

            /* 5% chance to launch slowDown iceCream */
            if (Random.value < 0.05f)
            {
                int powerUpConeIndex = Random.Range(0, iceCones.Count);
                Vector3 spawnPosition = iceCones[powerUpConeIndex].transform.position;
                while (spawnedPositions.Contains(spawnPosition))
                {
                    spawnPosition += Vector3.up * 1f; // Offset the position slightly
                }
                spawnedPositions.Add(spawnPosition);

                GameObject slowDownIceCream = Instantiate(slowDownSpeed, spawnPosition, Quaternion.identity);
                Rigidbody2D slowDownRigidbody = slowDownIceCream.GetComponent<Rigidbody2D>();
                slowDownRigidbody.AddForce(Vector2.up * iceCreamSpeed, ForceMode2D.Impulse);
            }
            /* 5% chance to launch gold +50 points iceCream */
            if (Random.value < 0.05f)
            {
                int goldIceCreamIndex = Random.Range(0, iceCones.Count);
                Vector3 spawnPosition = iceCones[goldIceCreamIndex].transform.position;
                while (spawnedPositions.Contains(spawnPosition))
                {
                    spawnPosition += Vector3.up * 1f; // Offset the position slightly
                }
                spawnedPositions.Add(spawnPosition);

                GameObject goldIceCream = Instantiate(iceCream_Three, spawnPosition, Quaternion.identity);
                Rigidbody2D gold_IceCream = goldIceCream.GetComponent<Rigidbody2D>();
                gold_IceCream.AddForce(Vector2.up * iceCreamSpeed, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(iceCreamCooldown);

            /* When amount of iceCreams have been destroyed -> Go next level */
            if (iceCreamDestroyed >= iceCreamAmount)
            {
                PlayerPrefs.SetInt("LevelOneScore", playerScore);
                logic.advanceToNextLevel();
            }
        }
    }
}

/* LogicScript deletes link to powerUpShield prefab when it spawns, troubleshoot later */
/* if (Random.value < 0.05f)
{
    int powerUpShieldIndex = Random.Range(0, iceCones.Count);
    GameObject powerUpShieldObj = Instantiate(powerUpShield, iceCones[powerUpShieldIndex].transform.position, Quaternion.identity);
    Rigidbody2D powerUpRigidBody = powerUpShieldObj.GetComponent<Rigidbody2D>();
    powerUpRigidBody.AddForce(Vector2.up * iceCreamSpeed, ForceMode2D.Impulse);
} */