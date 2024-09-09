using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPointer : MonoBehaviour
{

    private bool isMouseButtonPressed = false;

    public GameObject planePrefab; // Assign a plane prefab in the inspector
    private GameObject spawnedPlane; // To keep track of the spawned plane
    private Transform player; // Reference to the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check if the left mouse button is pressed and hold the state
        isMouseButtonPressed = Input.GetMouseButton(0);

        // If the plane is spawned but the mouse button is released, destroy the plane
        if (spawnedPlane != null && !isMouseButtonPressed)
        {
            Destroy(spawnedPlane);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered with: " + other.gameObject.name);
        // Only execute if the left mouse button is pressed
        if (isMouseButtonPressed)
        {
            if (other.CompareTag("Obstacle"))
            {
                //Debug.Log("Entered a trigger with an obstacle!");
                // Spawn the plane at the point of collision
                if (spawnedPlane == null)
                {
                    spawnedPlane = Instantiate(planePrefab, other.ClosestPoint(transform.position), Quaternion.identity);
                    spawnedPlane.transform.LookAt(player); // Make the plane face the player
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Destroy the plane when the objects stop interacting
        if (spawnedPlane != null && other.CompareTag("Obstacle"))
        {
            Destroy(spawnedPlane);
        }
    }

}