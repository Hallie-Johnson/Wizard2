using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSkurge : MonoBehaviour
{
    private float shrinkDuration = 0.75f;  // Time it takes to shrink the object to nothing

    private Coroutine shrinkCoroutine;

    // Call this method to start the shrinking process
    public void StartShrinking()
    {
        if (shrinkCoroutine != null)
        {
            StopCoroutine(shrinkCoroutine); // Stop any previous shrinking coroutine
        }
        shrinkCoroutine = StartCoroutine(ShrinkAndDestroyObject());
    }

    private IEnumerator ShrinkAndDestroyObject()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        // Gradually shrink the object over time
        while (elapsedTime < shrinkDuration)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
            transform.localScale = new Vector3(
                originalScale.x * scale,  // Only scale X
                originalScale.y,          // Keep Y constant
                originalScale.z * scale   // Only scale Z
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is correct
        transform.localScale = new Vector3(
            0f,                         // X scale is zero
            originalScale.y,            // Y scale remains the same
            0f                          // Z scale is zero
        );
        Destroy(gameObject);
    }
}
