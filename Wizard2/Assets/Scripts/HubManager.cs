using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public GameObject challenge1Object;
    private GameObject challenge2Object;
    public GameObject challenge3Object;
    public GameObject challenge4Object;

    public Canvas challenge1Canvas;
    public Canvas challenge2Canvas;
    public Canvas challenge3Canvas;
    public Canvas challenge4Canvas;

    public GameObject skurgePrefab;

    void Update()
    {
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

        challenge1Canvas.gameObject.SetActive(false);
        challenge2Canvas.gameObject.SetActive(false);
        challenge3Canvas.gameObject.SetActive(false);
        challenge4Canvas.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
