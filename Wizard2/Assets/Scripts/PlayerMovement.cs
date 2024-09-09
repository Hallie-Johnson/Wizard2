using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 250;
    private float jumpForce = 5f; // Force applied when jumping
    public bool isGrounded = true; // To check if the player is on the ground
    private Rigidbody rb;


    public Camera freeLookCamera1;
    public float rotationSmoothTime = 0.1f;


    public Camera playerCamera;  // Assign your camera here
    public float rayDistance = 100f;  // Maximum distance for the raycast

    public GameObject planePrefab;  // Assign your plane prefab here
    private float planeDistanceFromCamera = 3f;  // Distance to spawn the plane in front of the obstacle
    private GameObject spawnedPlane;  // Reference to the spawned plane

    public GameObject playerObject;
    public float heightOffset = 1f;

    public List<Material> materials;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        #region Look at Spell Object

        // Cast a ray from the center of the camera's viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Get all objects the ray hits
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

        // Variable to track if an obstacle is hit
        bool hitObstacle = false;
        RaycastHit hitObstacleInfo = default;  // Store information about the hit obstacle

        // Loop through the hits and ignore objects tagged as "Player" or "Ground"
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Ground"))
            {
                // Skip objects with the "Player" or "Ground" tag
                continue;
            }

            #region Spell Checker
            if (hit.collider.CompareTag("Rictusempra"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[0];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Skurge"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[1];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Diffindo"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[2];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Spongify"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[3];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Lumos"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[4];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Alohomora"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[5];
                break; // Exit loop after hitting the first valid obstacle
            }

            if (hit.collider.CompareTag("Flipendo"))
            {
                hitObstacle = true;
                hitObstacleInfo = hit;  // Store the hit information
                material = materials[6];
                break; // Exit loop after hitting the first valid obstacle
            }
            #endregion
        }

        if (Input.GetMouseButton(0) && hitObstacle) // Check if the left mouse button is held down and an obstacle is hit
        {
            // Spawn the plane if not already spawned
            if (spawnedPlane == null)
            {
                // Calculate the midpoint between the player and the hit obstacle
                Vector3 midpoint = (playerObject.transform.position + hitObstacleInfo.point) / 2;

                // Adjust the Y position to raise the plane slightly above the midpoint
                midpoint.y += heightOffset;

                // Instantiate the plane at the adjusted midpoint
                spawnedPlane = Instantiate(planePrefab, midpoint, Quaternion.identity);

                Renderer renderer = spawnedPlane.GetComponent<Renderer>();
                renderer.material = material;

                MeshRenderer meshRenderer = spawnedPlane.GetComponent<MeshRenderer>();
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            // Ensure the plane always faces the camera
            if (spawnedPlane != null)
            {
                Vector3 directionToCamera = playerCamera.transform.position - spawnedPlane.transform.position;
                Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
                spawnedPlane.transform.rotation = rotationToCamera;
            }
        }
        else if (!Input.GetMouseButton(0) || !hitObstacle) // Check if the left mouse button is released or no obstacle is hit
        {
            if (spawnedPlane != null)
            {
                // Destroy the plane if it exists
                Destroy(spawnedPlane);
                spawnedPlane = null;
            }
        }

        #endregion

        #region Player Movement

        // Get input for both vertical and horizontal movement
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Define a small threshold to prevent floating-point precision issues
        float inputThreshold = 0.01f;

        // Only move if input exceeds the threshold
        if (Mathf.Abs(moveVertical) > inputThreshold || Mathf.Abs(moveHorizontal) > inputThreshold)
        {
            // Combine both movements into a single vector
            Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed * Time.deltaTime;

            // Apply movement in a single MovePosition call
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            // If no significant input, stop the player completely

            // 1. Reset velocity to zero
            //rb.velocity = Vector3.zero;

            // 2. Reset angular velocity to prevent unwanted rotation
            //rb.angularVelocity = Vector3.zero;

            // 3. Force the Rigidbody to sleep to prevent any further physics updates
            //rb.Sleep();
        }


        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }


        #endregion

    }

    // Check if the player is grounded by detecting collisions with the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set to true when touching the ground
        }

        if (gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
            rb.velocity = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        if (rb == null || freeLookCamera1 == null) return;

        // Get the camera's Y rotation
        float cameraYRotation = freeLookCamera1.transform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraYRotation, 0);

        // Smoothly rotate the player's Rigidbody
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothTime));
    }

}

