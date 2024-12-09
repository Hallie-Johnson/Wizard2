using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Star : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    private bool isCollected = false;
    public Canvas victoryCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            animator.SetBool("isCollected", true); // Set the parameter to true


            if (GameManager.Instance != null)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                //Debug.Log("Current Scene Name: " + currentSceneName);
                if (currentSceneName.Equals("Hub"))
                {
                    if (victoryCanvas != null)
                    {
                        victoryCanvas.gameObject.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
            }


            StartCoroutine(WaitForAnimationAndDestroy());

            
        }
    }

    private System.Collections.IEnumerator WaitForAnimationAndDestroy()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
