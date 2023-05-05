using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public AudioClip buttonClickOk;
    public LogicScript logic;

    public void goToNextScene()
    {
        AudioSource buttonClickOk = GetComponent<AudioSource>();
        buttonClickOk.Play();
        StartCoroutine(LoadNextSceneWithDelay());
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Level One");
        Time.timeScale = 1.0f;
    }
}
