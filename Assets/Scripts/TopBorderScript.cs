using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBorderScript : MonoBehaviour
{
    public LogicScript logic;
    public IceCreamShooter spawner;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   if (!collision.gameObject.CompareTag("PlayerModel"))
        {
            if (SceneManager.GetActiveScene().name == "Level One")
            {   
                logic.addScore(1);
                Destroy(collision.gameObject);
                spawner.iceCreamDestroyed++;
            }
            if (SceneManager.GetActiveScene().name == "Level Two")
            {
                logic.addScore(2);
                Destroy(collision.gameObject);
                spawner.iceCreamDestroyed++;
            }
            if (SceneManager.GetActiveScene().name == "Level Three")
            {
                logic.addScore(3);
                Destroy(collision.gameObject);
                spawner.iceCreamDestroyed++;
            }
        }
    }
}
 