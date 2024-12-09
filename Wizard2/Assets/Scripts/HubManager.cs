using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HubManager : MonoBehaviour
{
    public GameObject player;
    public GameObject challenge1Object;
    private GameObject challenge2Object;
    public GameObject challenge3Object;
    public GameObject challenge4Object;

    public GameObject victoryObject;
    public GameObject victoryStarObject;

    public Canvas challenge0Canvas;
    public Canvas challenge1Canvas;
    public Canvas challenge2Canvas;
    public Canvas challenge3Canvas;
    public Canvas challenge4Canvas;

    public Canvas passedCanvas;
    public Canvas failedCanvas;
    public Canvas instructionsCanvas;
    private bool showedInstructions;

    public TextMeshProUGUI gradeC1Text;
    public TextMeshProUGUI gradeC2Text;
    public TextMeshProUGUI gradeC3Text;
    public TextMeshProUGUI gradeC4Text;

    public GameObject skurgePrefab;

    void Start()
    {
        victoryObject.SetActive(false);
        failedCanvas.gameObject.SetActive(false);
        passedCanvas.gameObject.SetActive(false);
        showedInstructions = false;


        if (GameManager.Instance != null)
        {
            gradeC1Text.text = gradeC1Text.text + GameManager.Instance.c1_grade;
            gradeC2Text.text = gradeC2Text.text + GameManager.Instance.c2_grade;
            gradeC3Text.text = gradeC3Text.text + GameManager.Instance.c3_grade;
            gradeC4Text.text = gradeC4Text.text + GameManager.Instance.c4_grade;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.c1_grade.Equals("N/A") || GameManager.Instance.c2_grade.Equals("N/A") || GameManager.Instance.c3_grade.Equals("N/A") || GameManager.Instance.c4_grade.Equals("N/A"))
            {
                if (GameManager.Instance.c1_grade.Equals("N/A") && GameManager.Instance.c2_grade.Equals("N/A") && GameManager.Instance.c3_grade.Equals("N/A") && GameManager.Instance.c4_grade.Equals("N/A") && showedInstructions == false)
                {
                    enableCursor();
                    instructionsCanvas.gameObject.SetActive(true);
                    showedInstructions = true;
                }
            } 
            else
            {
                if (GameManager.Instance.c1_grade.Equals("A") && GameManager.Instance.c2_grade.Equals("A") && GameManager.Instance.c3_grade.Equals("A") && GameManager.Instance.c4_grade.Equals("A"))
                {
                    victoryObject.SetActive(true);
                }
                else if (GameManager.Instance.c1_grade.Equals("F") || GameManager.Instance.c2_grade.Equals("F") || GameManager.Instance.c3_grade.Equals("F") || GameManager.Instance.c4_grade.Equals("F"))
                {
                    enableCursor();
                    failedCanvas.gameObject.SetActive(true);
                }
                else
                {
                    enableCursor();
                    passedCanvas.gameObject.SetActive(true);
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            enableCursor();
            instructionsCanvas.gameObject.SetActive(true);
        }

        float flipendoZ = victoryObject.transform.position.z;
        if (flipendoZ > 95)
        {
            if (victoryStarObject != null)
            {
                victoryStarObject.SetActive(true);
            }
        }


        if (player != null) // Ensure the player object exists
        {
            float playerZ = player.transform.position.z; // Get the player's z value

            if (playerZ < 49)
            {
                challenge0Canvas.gameObject.SetActive(true);
                enableCursor();

                Vector3 newPosition = player.transform.position; // Get the current position
                newPosition.z += 1f; // Add 5 units to the Z position
                newPosition.y = 1f;
                player.transform.position = newPosition; // Set the new position
            }
        }


        challenge2Object = GameObject.Find("Skurge");
        //Debug.Log(GameObject.Find("Skurge"));

        // Check Challenge 1: Detect if "isStunned" is true
        if (challenge1Object != null)
        {
            ManagerRictusempra challenge1Script = challenge1Object.GetComponent<ManagerRictusempra>();
            if (challenge1Script != null && challenge1Script.isStunned)
            {
                challenge1Canvas.gameObject.SetActive(true);
                enableCursor();
            }
        }

        // Check Challenge 2: Detect if the object doesn't exist anymore
        if (challenge2Object == null)
        {
            challenge2Canvas.gameObject.SetActive(true);
            enableCursor();
        }

        // Check Challenge 3: Detect if the object is inactive
        if (challenge3Object != null)
        {
            if (!challenge3Object.activeSelf)
            {
                challenge3Canvas.gameObject.SetActive(true);
                enableCursor();
            }
        }

        // Check Challenge 4: Detect if "enableJumping" is true
        if (challenge4Object != null)
        {
            ManagerSpongify challenge4Script = challenge4Object.GetComponent<ManagerSpongify>();
            if (challenge4Script != null && challenge4Script.enableJumping)
            {
                challenge4Canvas.gameObject.SetActive(true);
                enableCursor();
            }
        }
    }

    private void enableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayC1()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayC2()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void PlayC3()
    {
        SceneManager.LoadSceneAsync(4);
        DynamicGI.UpdateEnvironment();
    }

    public void PlayC4()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void closePopup()
    {
        Debug.Log("CLOSING");

        ManagerRictusempra challenge1Script = challenge1Object.GetComponent<ManagerRictusempra>();
        challenge1Script.isStunned = false;

        if (challenge2Object == null)
        {
            Vector3 spawnPosition = new Vector3(12f, 3f, 66f);
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 90f);
            Vector3 spawnScale = new Vector3(6.5f, 1.8f, 6.5f);
            GameObject skurgeInstance = Instantiate(skurgePrefab, spawnPosition, spawnRotation);
            skurgeInstance.name = "Skurge";
            skurgeInstance.transform.localScale = spawnScale;
        }

        challenge3Object.SetActive(true);


        ManagerSpongify challenge4Script = challenge4Object.GetComponent<ManagerSpongify>();
        challenge4Script.enableJumping = false;

        instructionsCanvas.gameObject.SetActive(false);

        challenge0Canvas.gameObject.SetActive(false);
        challenge1Canvas.gameObject.SetActive(false);
        challenge2Canvas.gameObject.SetActive(false);
        challenge3Canvas.gameObject.SetActive(false);
        challenge4Canvas.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
