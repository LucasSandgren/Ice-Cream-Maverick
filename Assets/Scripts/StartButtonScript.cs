using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public AudioClip buttonClickOk;
    public LogicScript logic;
    
  
     public void startGame()
    {
        Debug.Log("WHATS HAPPENING");
        AudioSource buttonClickOk = GetComponent<AudioSource>();
        buttonClickOk.Play();
        SceneManager.LoadScene("Level One");
        Time.timeScale = 1.0f;
        // StartCoroutine(LoadNextSceneWithDelay()); // ADD DELAY BEFORE GAME START WHEN CLICKING START, NOT NEEDED
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level One");
        Time.timeScale = 1.0f;
    } 
}
