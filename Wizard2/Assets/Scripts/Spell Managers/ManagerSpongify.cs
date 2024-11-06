using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpongify : MonoBehaviour
{
    public Transform targetPlatform;  // Target platform where the player should land
    public float launchTime = 1.5f;   // Time it should take to reach the target
    private float heightAdjustment = 1.0f;  // Adjusts target Y to ensure landing
    public float gravityModifier = 1.0f;  // Gravity adjustment for the arc

    public GameObject player;  // Reference to the player object
    private CharacterController playerController;  // Reference to the player's CharacterController

    private Vector3 velocity;  // Player's velocity to apply movement
    private bool isLaunching = false;  // Flag to check if launch is in progress
    private float launchStartTime;  // The time when the launch starts
    private Vector3 startPosition;  // Starting position of the player
    private Vector3 targetPosition;  // Target position to launch towards

    void Start()
    {
        // Ensure the player object has a CharacterController
        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
        }

        // Adjust gravity if necessary for a better arc
        Physics.gravity *= gravityModifier;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Make sure we are hitting the player
        {
            Debug.Log("Triggered with object tag: " + other.gameObject.tag);
            LaunchPlayer();
        }
    }

    void LaunchPlayer()
    {
        // Get the target position (platform position with height adjustment)
        targetPosition = targetPlatform.position;
        targetPosition.y += heightAdjustment;

        // Set the starting position of the player
        startPosition = player.transform.position;

        // Set the velocity for the launch
        velocity = CalculateLaunchVelocity();

        // Start the launch process
        isLaunching = true;

        // Record the time the launch starts
        launchStartTime = Time.time;
    }

    Vector3 CalculateLaunchVelocity()
    {
        // Calculate the distance and time for the launch
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

    void Update()
    {
        if (isLaunching)
        {
            // Calculate how much time has passed since the launch started
            float elapsedTime = Time.time - launchStartTime;

            // If the player has reached the target (or the launch time is over), stop the launch
            if (elapsedTime >= launchTime)
            {
                isLaunching = false;
                return;
            }

            // Calculate the proportional progress of the launch
            float launchProgress = elapsedTime / launchTime;

            // Calculate the new position by interpolating between the start and target positions
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, launchProgress);

            // Apply gravity manually for a more natural arc
            velocity.y += Physics.gravity.y * Time.deltaTime;

            // Apply the movement based on the velocity
            playerController.Move(velocity * Time.deltaTime);

            // Set the player's position closer to the target platform
            player.transform.position = newPosition;
        }
    }





    /*
    public Transform targetPlatform;  // Target platform where the player should land
    public float launchTime = 1.5f;   // Time it should take to reach the target
    private float heightAdjustment = 1.0f;  // Adjusts target Y to ensure landing
    public float gravityModifier = 1.0f;  // Gravity adjustment for the arc
    private Rigidbody playerRb;

    void Start()
    {
        // Adjust gravity if necessary for a better arc
        Physics.gravity *= gravityModifier;
    }

    // For trigger collisions (if your collider is set to Trigger)
    void OnTriggerEnter(Collider other)
    {
        playerRb = other.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Debug.Log("Triggered with object tag: " + other.gameObject.tag);
            LaunchPlayer();
        }
    }

    void LaunchPlayer()
    {
        Vector3 launchVelocity = CalculateLaunchVelocity();
        playerRb.velocity = launchVelocity;  // Set the player's velocity to the calculated value
        Debug.Log("Launch Player" + launchVelocity);
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


    /*
    
    
    
    

    public bool enableJumping = false;

    

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.CompareTag("Player") && enableJumping == true)
        {
            PlayerMovement1 playerMovement = other.GetComponent<PlayerMovement1>();

            if (playerMovement != null && !playerMovement.isGrounded)
            {
                playerRb = other.GetComponent<Rigidbody>();
                LaunchPlayer();
            }
        }
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
    */

}
