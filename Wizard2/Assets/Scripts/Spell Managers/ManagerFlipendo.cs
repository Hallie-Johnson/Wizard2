using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFlipendo : MonoBehaviour
{
    public float pushDistance = 5f;  // Distance to push the object
    public float pushSpeed = 2f;  // Speed of the push
    public float smoothFactor = 0.1f;  // Smoothing factor for movement

    private bool isPushing = false;
    private Vector3 pushDirection;
    private GameObject hitObject;
    private Rigidbody rb;
    private Vector3 originalPosition;
    private Vector3 pushTarget;
    private float pushStartTime;
    private float initialY;  // Store the initial Y-level

    void Start()
    {
        // Get the Rigidbody attached to the object
        rb = GetComponent<Rigidbody>();
        rb.drag = 10f;  // Adjust drag to prevent sliding
        rb.angularDrag = 10f;  // Adjust angular drag to prevent rotations

        // Store initial Y-level
        initialY = transform.position.y;
    }

    // Method to start pushing the object
    public void StartPush(GameObject objectToPush, Vector3 pushDirection)
    {
        hitObject = objectToPush;
        this.pushDirection = pushDirection;

        // Calculate the target position to push the object
        originalPosition = hitObject.transform.position;
        pushTarget = originalPosition + pushDirection * pushDistance;

        // Start pushing
        isPushing = true;

        // Set push start time
        pushStartTime = Time.time;

        // Ensure Rigidbody is not kinematic so it can move
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero; // Clear any existing velocity
        }
    }

    void FixedUpdate()
    {
        if (isPushing && hitObject != null)
        {
            // Calculate how far to push based on time
            float elapsedTime = Time.time - pushStartTime;
            float progress = Mathf.Clamp01(elapsedTime * pushSpeed / pushDistance);

            // Calculate target position with interpolation for smoother movement
            Vector3 targetPosition = Vector3.Lerp(originalPosition, pushTarget, progress);

            // Keep the Y-level constant
            targetPosition.y = initialY;

            // Smoothly move the object
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, smoothFactor));

            // Stop pushing if the object is close enough to the target
            if (Vector3.Distance(hitObject.transform.position, pushTarget) < 0.1f)
            {
                isPushing = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object has the tag "Wall" or "Skurge"
        if (isPushing && (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Skurge")))
        {
            // Stop pushing if the object hits something with the tag
            isPushing = false;

            // Stop the object's movement by setting velocity to zero
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

}
