using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerModelScript : MonoBehaviour
{
    public LogicScript logic;
    public IceCreamShooter spawner;

    /* Variables for Player Model */
    public bool playerModelAlive = true;
    public int currentHealth = 3;
    public int maxHealth = 3;
    public float duration = 5f;

    /* Variables for Current Health Text */
    public Text healthText;

    /* Scene Management variables */
    private Scene activeScene;

    /* Variables for shield Power Up */
    public float shieldDuration = 5f;
     // Add sound to powerup LATER

    /* Variables for SFX */
    public AudioSource playerModelOnHit;
    public AudioSource shieldSound;
    public AudioSource slowIceCream;
    public AudioSource bonusIceCream;

    /* Variables for movement -->  THIS IS FOR WASD MOVEMENT WITH INPUT SYSTEM
    public float walkSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb; 
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * walkSpeed;
    } 

    /*public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput.y = Mathf.Clamp(moveInput.y, -1f, 1f); // clamp the y value between -1 and 1
        rb.velocity = moveInput * walkSpeed;
    } */

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        currentHealth = maxHealth;
        /* SFX */
        AudioSource playerModelOnHit = GetComponent<AudioSource>();
        AudioSource slowIceCream = GetComponent<AudioSource>();
        AudioSource bonusIceCream = GetComponent<AudioSource>();
        
        
        healthText.text = "3";
        activeScene = SceneManager.GetActiveScene();

    }


    void Update()
    {   
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        /* Get screen boundaries */
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        float xMax = screenBounds.x;
        float yMax = screenBounds.y;

        /* Limit the position of the PlayerModel within the screen bounds */
        float x = Mathf.Clamp(mousePosition.x, -xMax, xMax);
        float y = Mathf.Clamp(mousePosition.y, -yMax, yMax);
        Vector3 clampedPosition = new Vector3(x, y, 0f);

        transform.position = clampedPosition;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.CompareTag("BonusPoints"))
        {
            bonusIceCream.Play();
            logic.addScore(50);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("powerUps"))
        {
            slowIceCream.Play();
            StartCoroutine(SlowDownCoroutine(5f));
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("IceCream"))
        {
            playerModelOnHit.Play();
            currentHealth--;

            if (currentHealth == 3)
            {
                healthText.color = Color.green;
                healthText.text = currentHealth.ToString();
            }
            if (currentHealth == 2)
            {
                healthText.color = Color.yellow;
                healthText.text = currentHealth.ToString();
            }
            if (currentHealth == 1)
            {
                healthText.color = Color.red;
                healthText.text = currentHealth.ToString();
            }
        } 
            
        Destroy(collision.gameObject);
        if (currentHealth <= 0 && playerModelAlive)
        {
            logic.gameOver();
            playerModelAlive = false;
            Destroy(gameObject);
            Cursor.visible = true;
            Time.timeScale = 0f;
            healthText.text = "0";
        }

        /* if (collision.gameObject.CompareTag("powerUpShield"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(ApplyShield());
        } */
    }
    
    /* IEnumerator ApplyShield()
    {
        currentHealth = 999;
        healthText.color = Color.blue;
        healthText.text = currentHealth.ToString();
        yield return new WaitForSeconds(duration);

        currentHealth = 3;
        healthText.color = Color.green;
        healthText.text = currentHealth.ToString();
    } */

    
    

    IEnumerator SlowDownCoroutine(float slowDuration)
    {
        
        if (activeScene.name == "Level One")
        {
            spawner.iceCreamSpeed = 3f;
            yield return new WaitForSeconds(slowDuration);
            spawner.iceCreamSpeed = 5f;
        }
        if (activeScene.name == "Level Two")
        {
            spawner.iceCreamSpeed = 5f;
            yield return new WaitForSeconds(slowDuration);
            spawner.iceCreamSpeed = 7f;
        }
        if (activeScene.name == "Level Three")
        {
            spawner.iceCreamSpeed = 7f;
            yield return new WaitForSeconds(slowDuration);
            spawner.iceCreamSpeed = 10f;
        }
    }
}