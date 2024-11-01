using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Platform : MonoBehaviour
{
    private GameObject player; // Reference to the player object
    public float transform_amount = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject; // Store the player reference
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null; // Clear the player reference
        }
    }

    private void Update()
    {
        // If the player is on the cube, match their Z position to the cube's Z position
        if (player != null)
        {
            Vector3 newPlayerPosition = player.transform.position;
            newPlayerPosition.z = transform.position.z - transform_amount; // Match the cube's Z position
            player.transform.position = newPlayerPosition;
        }
    }
}
