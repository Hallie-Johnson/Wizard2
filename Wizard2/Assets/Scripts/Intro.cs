using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    void Start()
    {
        // Start the coroutine when the game starts
        StartCoroutine(WaitAndPrint());
    }

    IEnumerator WaitAndPrint()
    {
        // Wait for 17 seconds
        yield return new WaitForSeconds(18);

        // Perform your action here
        SceneManager.LoadSceneAsync(1);
    }
}
