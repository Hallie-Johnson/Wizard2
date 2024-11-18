using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4Rotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // The speed at which the object rotates, in degrees per second

    void Update()
    {
        // Rotate the object on its X axis at the given speed
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
    }

   
}
