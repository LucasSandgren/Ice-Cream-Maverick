using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public LogicScript logic;
    public IceCreamShooter spawner;

    /* Variable for calculating player score */
    public int playerScore;
    public Text scoreText;
    public GameObject playerModel;
   
    /* Variables for Power Ups */
    public GameObject powerUpShield; // ADD LATER
    
    /* Variables for spawn and speed for flying objects */
    public int iceCreamAmount = 75;

    /* Variables for Game Over Screen / Next Level Screen */
    public GameObject gameOverScreen;
    public GameObject endScore;
    public GameObject nextLevelButton;
    public GameObject nextLevelScreen;
    public Text nextLevelScore;
    public Text playerEndScore;
    private Scene activeScene;

    public bool playerModelAlive = true;

    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<IceCreamShooter>();
    }
    public void addScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        playerModelAlive = false;
        scoreText.enabled = false;
        playerEndScore.text = "SCORE: " + playerScore.ToString();
    }

    public void advanceToNextLevel()
    {
        Time.timeScale = 0f;
        nextLevelScreen.SetActive(true);
        nextLevelButton.SetActive(true);
        playerModelAlive = false;
        Cursor.visible = true;
        nextLevelScore.text = "SCORE: " + playerScore.ToString();
        spawner.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void nextLevel()
    {
        int levelScore = PlayerPrefs.GetInt("LevelOneScore", 0);
        int totalScore = playerScore + levelScore;

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level One")
        {
            SceneManager.LoadScene("Level Two");
        }
         if (currentScene.name == "Level Two")
        {
            SceneManager.LoadScene("Level Three");
        }
        nextLevelScore.text = "SCORE: " + totalScore.ToString();
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f; // Resume game speed
        scoreText.enabled = true;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Start Screen");
        Cursor.visible = true;
        
    }
} 







