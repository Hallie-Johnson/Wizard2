using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C3StarFollower : MonoBehaviour
{
    public GameObject starObject; // Reference to the star object
    public float heightAboveObject = 2.0f; // Desired height above this object

    private void Update()
    {
        if (starObject != null)
        {
            // Get the star's current position
            Vector3 starPosition = starObject.transform.position;

            // Set the new Y position, maintaining the original X and Z
            starObject.transform.position = new Vector3(starPosition.x, transform.position.y + heightAboveObject, starPosition.z);
        }
    }
}
