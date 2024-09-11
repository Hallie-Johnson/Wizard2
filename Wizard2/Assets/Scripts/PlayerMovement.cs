using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;


public class PlayerMovement : MonoBehaviour
{
    // This script is used player movement

    public float speed = 5f;
    public float turnSpeed = 250;
    private float jumpForce = 6f; // Force applied when jumping
    public bool isGrounded = true; // To check if the player is on the ground
    private Rigidbody rb;

    public Camera freeLookCamera1;
    private float rotationSmoothTime = 1f; //0.1f;
    
    public Camera playerCamera;  // Assign your camera here



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

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

