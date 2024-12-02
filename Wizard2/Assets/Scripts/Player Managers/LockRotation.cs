using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    void Update()
    {
        // Get the current rotation of the object
        Quaternion currentRotation = transform.rotation;

        // Lock the x and z rotation to 0, keep y unchanged
        //transform.rotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);
    }
}
