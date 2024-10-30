using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFlipendoButton : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;

    // Duration for the rotation to complete
    public float rotationDuration = 1.0f;

    // Cooldown time in seconds
    public float cooldownTime = 2.0f;
    private float lastRotateTime = 0.0f;

    // Function to rotate the objects smoothly
    public void RotateObjects()
    {
        // Check if enough time has passed since the last rotation
        if (Time.time - lastRotateTime < cooldownTime)
        {
            return; // Exit the function if still in cooldown
        }

        lastRotateTime = Time.time; // Update the last rotate time

        StartCoroutine(RotateCoroutine(object1, new Vector3(90, 0, 0)));
        StartCoroutine(RotateCoroutine(object2, new Vector3(0, 0, 90)));
        StartCoroutine(RotateCoroutine(object3, new Vector3(90, 0, 0)));
        StartCoroutine(RotateCoroutine(object4, new Vector3(0, 0, 90)));
    }

    // Coroutine to handle the smooth rotation
    private IEnumerator RotateCoroutine(GameObject obj, Vector3 rotationVector)
    {
        Quaternion startRotation = obj.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationVector);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            // Calculate the percentage of time completed
            float t = elapsedTime / rotationDuration;
            // Smoothly interpolate to the target rotation
            obj.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Set to the target rotation directly without snapping
        obj.transform.rotation = targetRotation;
    }
}
