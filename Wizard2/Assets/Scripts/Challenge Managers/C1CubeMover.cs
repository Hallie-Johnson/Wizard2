using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1CubeMover : MonoBehaviour
{
    private float moveDistance = 7.0f; // Distance to move forward
    public float speed = 1.0f; // Speed at which the object moves forward and backward

    private Vector3 initialPosition;
    private bool movingForward = true;
    private float pauseDuration;

    private void Start()
    {
        initialPosition = transform.position; // Record the starting position
        StartCoroutine(MoveObjectRoutine());

        // Ensure this object has a Rigidbody for collision detection
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Make it kinematic so it doesn't react to physics forces
        }
    }

    private IEnumerator MoveObjectRoutine()
    {
        while (true)
        {
            // Calculate pause duration based on the current speed
            pauseDuration = 1.0f / speed;

            // Set the target position based on direction
            Vector3 targetPosition = movingForward ? initialPosition + transform.forward * moveDistance : initialPosition;

            // Move the object toward the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Pause at the target position
            yield return new WaitForSeconds(pauseDuration);

            // Reverse direction for the next cycle
            movingForward = !movingForward;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("here");
            // Calculate the push direction (away from the cube)
            Vector3 pushDirection = collision.transform.position - transform.position;
            pushDirection.Normalize(); // Normalize to get a unit vector

            // Apply a force to the player object
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                float pushForce = 20.0f; // Adjust the force as needed
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
