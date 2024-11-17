using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            animator.SetBool("isCollected", true); // Set the parameter to true
            StartCoroutine(WaitForAnimationAndDestroy());
        }
    }

    private System.Collections.IEnumerator WaitForAnimationAndDestroy()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
