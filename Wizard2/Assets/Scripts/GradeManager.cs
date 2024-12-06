using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GradeManager : MonoBehaviour
{
    // Timer variables
    public float timerInSeconds = 500f; // Set the timer duration
    private float currentTime;

    // UI Text objects
    public TextMeshProUGUI timerText; // Reference to the timer TextMeshPro object
    public TextMeshProUGUI gradeText; // Reference to the grade TextMeshPro object
    public TextMeshProUGUI cardText; // Reference to the card TextMeshPro object

    // Card Value
    private float cardValue;

    // Grade thresholds
    private float gradeAThreshold;
    private float gradeBThreshold;
    private float gradeCThreshold;
    private float gradeDThreshold;

    // Player-related references
    public GameObject player; // Reference to the player GameObject
    public Animator playerAnimator; // Animator for the player
    private ThirdPersonMovement playerMovement; // Reference to the player's movement script

    // Canvas object to enable on "F"
    public GameObject failCanvas;



    void Start()
    {
        currentTime = timerInSeconds; // Initialize the timer

        gradeAThreshold = (timerInSeconds / 4) * 3;
        gradeBThreshold = (timerInSeconds / 4) * 2;
        gradeCThreshold = (timerInSeconds / 4) * 1;
        gradeDThreshold = (timerInSeconds / 4) * 0;

        cardValue = (timerInSeconds / 4);
        cardText.text = "--";


        // Get references to player components
        if (player != null)
        {
            playerMovement = player.GetComponent<ThirdPersonMovement>();
        }

        // Ensure the fail canvas is initially inactive
        if (failCanvas != null)
        {
            failCanvas.SetActive(false);
        }


        UpdateUI(); // Update UI at start
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Decrease time
            if (currentTime < 0) currentTime = 0; // Prevent negative time
            UpdateUI(); // Update the UI with the current time and grade
        }
    }

    void UpdateUI()
    {
        // Update the timer text
        if (timerText != null)
        {
            timerText.text = $"TIME {Mathf.CeilToInt(currentTime)}";
        }

        // Update the grade text
        if (gradeText != null)
        {
            gradeText.text = GetGrade();
        }

        if (GetGrade() == "F")
        {
            HandleFailState();
        }
    }

    string GetGrade()
    {
        if (currentTime >= gradeAThreshold) return "A";
        if (currentTime >= gradeBThreshold) return "B";
        if (currentTime >= gradeCThreshold) return "C";
        if (currentTime > gradeDThreshold) return "D";
        return "F";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            currentTime = currentTime + cardValue;
            other.tag = "Untagged";
        }

        cardText.text = "+";
    }

    void HandleFailState()
    {
        // Trigger the fail animation
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("isFail");
        }

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.movement = false; // Assuming `canMove` exists in ThirdPersonMovement
        }

        // Disable the HealthManager script
        if (player != null)
        {
            HealthManager healthManager = player.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.enabled = false;
            }
        }

        StartCoroutine(failscreen());

        
    }

    IEnumerator failscreen()
    {
        yield return new WaitForSeconds(4.5f);

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

    public void LeaveExam()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
