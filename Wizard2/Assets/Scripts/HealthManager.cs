using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] sourceImages = new Sprite[7];
    public int health;
    public int health_max = 6;

    // Canvas object to enable on "F"
    public GameObject failCanvas;
    public GameObject passCanvas;

    public TextMeshProUGUI finalGradeText;
    public TextMeshProUGUI gradeText;
    public string finalGrade;

    public Animator playerAnimator; // Animator for the player
    private ThirdPersonMovement playerMovement; // Reference to the player's movement script
    public GameObject player; // Reference to the player GameObject

    private bool gameOver;

    private bool hitByDeath;



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Debug.Log("Player hit by a fireball!");
            DecreaseHealth(1);
        }

        if (collision.gameObject.CompareTag("Skurge"))
        {
            Debug.Log("Player hit by a skurge!");
            DecreaseHealth(1);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Player hit by a death!");
            hitByDeath = true;
            DecreaseHealth(health_max);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            Debug.Log("Player hit by a star!");
            HandleVictoryState();
        }
    }

    void Update()
    {
        targetImage.sprite = sourceImages[health];

        if (gameOver)
        {
            DestroyAllFireballs();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync(1);
            }
        }
    }

    void Start()
    {
        gameOver = false;
        hitByDeath = false;
        health = health_max;

        // Get references to player components
        if (player != null)
        {
            playerMovement = player.GetComponent<ThirdPersonMovement>();
        }

    }

    void DecreaseHealth(int value)
    {
        if (health <= 0 || value == health_max)
        {
            health = 0;
        }
        else health = health - value;

        if (health <= 0)
        {
            HandleDeathState();
        }
    }


    void HandleDeathState()
    {
        gameOver = true;

        // Trigger the fail animation
        if (playerAnimator != null && hitByDeath == false)
        {
            playerAnimator.SetTrigger("isDead");
        }

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.movement = false; // Assuming `canMove` exists in ThirdPersonMovement
        }

        // Disable the GradeManager script
        if (player != null)
        {
            GradeManager gradeManager = player.GetComponent<GradeManager>();
            if (gradeManager != null)
            {
                gradeManager.enabled = false;
            }
        }

        StartCoroutine(failscreen());
    }


    IEnumerator failscreen()
    {
        yield return new WaitForSeconds(2f);

        // Enable the fail canvas
        if (failCanvas != null)
        {
            failCanvas.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    void DestroyAllFireballs()
    {
        // Find all objects with the "Fireball" tag
        GameObject[] fireballs = GameObject.FindGameObjectsWithTag("Fireball");

        // Loop through and destroy each one
        foreach (GameObject fireball in fireballs)
        {
            Destroy(fireball);
        }
    }


    void HandleVictoryState()
    {
        gameOver = true;

        // Disable the GradeManager script
        if (player != null)
        {
            GradeManager gradeManager = player.GetComponent<GradeManager>();
            if (gradeManager != null)
            {
                gradeManager.enabled = false;
            }
        }

        StartCoroutine(passscreen());
    }


    IEnumerator passscreen()
    {
        yield return new WaitForSeconds(4f);

        // Enable the fail canvas
        if (passCanvas != null)
        {
            passCanvas.SetActive(true);
        }

        finalGradeText.text = gradeText.text;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
    }

}
