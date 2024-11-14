using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpongify : MonoBehaviour
{
    public Transform targetPlatform;  // Target platform where the player should land
    public float launchTime = 1.5f;   // Time it should take to reach the target
    public float gravityModifier = 1.0f;  // Gravity adjustment for the arc
    private float heightAdjustment = 1.0f;  // Adjusts target Y to ensure landing

    public bool enableJumping = false;

    private Rigidbody playerRb;

    void Start()
    {
        // Adjust gravity if necessary for a better arc
        Physics.gravity *= gravityModifier;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enableJumping == true)
        {
            ThirdPersonMovement playerMovement = other.GetComponent<ThirdPersonMovement>();

            if (playerMovement != null && !playerMovement.isGrounded)
            {
                playerRb = other.GetComponent<Rigidbody>();
                LaunchPlayer();
            }
        }
    }

    void LaunchPlayer()
    {
        Vector3 launchVelocity = CalculateLaunchVelocity();
        playerRb.velocity = launchVelocity;  // Set the player's velocity to the calculated value
    }

    Vector3 CalculateLaunchVelocity()
    {
        // Calculate the distance and time for the launch
        Vector3 targetPosition = targetPlatform.position;

        // Adjust the target Y position to ensure the player lands on top of the platform
        targetPosition.y += heightAdjustment;

        Vector3 startPosition = transform.position;

        Vector3 displacement = targetPosition - startPosition;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);  // Horizontal distance

        // Time to reach the target
        float timeToTarget = launchTime;

        // Calculate the horizontal velocity (XZ plane)
        Vector3 horizontalVelocity = displacementXZ / timeToTarget;

        // Calculate the required vertical velocity to reach the correct height
        float verticalVelocity = (displacement.y / timeToTarget) + (0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget);

        // Combine horizontal and vertical velocities
        Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        return launchVelocity;
    }

    public void EnableJumping()
    {
        enableJumping = true;
        StartCoroutine(DisableJumpingAfterTime(5f)); // Disable jumping after 5 seconds
    }

    IEnumerator DisableJumpingAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        enableJumping = false;  // Disable jumping after the delay
    }

}