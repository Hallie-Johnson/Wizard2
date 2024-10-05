using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{

    public float speed = 5f;
    public float turnSpeed = 250f;
    private float jumpForce = 10f; // Force applied when jumping
    public bool isGrounded = true; // To check if the player is on the ground
    private Rigidbody rb;
    private Vector3 movementVelocity;

    private Vector3 smoothVelocity; // Add this to store velocity for smoothing

    // Reference to the Animator
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        //animator = GetComponent<Animator>(); // Get the Animator component
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0)) // 0 is for left mouse button
        {
            // Trigger casting animation
            animator.SetBool("isCasting", true);
        }
        else if (Input.GetMouseButtonUp(0)) // When the button is released
        {
            // Stop casting animation and return to idle
            animator.SetBool("isCasting", false);
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Combine vertical and horizontal input to form movement vector
        Vector3 targetMovement = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed;

        // Smoothly interpolate to the target movement velocity
        movementVelocity = Vector3.SmoothDamp(movementVelocity, targetMovement, ref smoothVelocity, 0.1f); // Adjust time as needed

        // Apply movement using Rigidbody's MovePosition
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);

        // Check if the player is moving
        bool isMoving = ((moveVertical != 0 || moveHorizontal != 0) && !Input.GetMouseButton(0)); // Check if there is any input

        // Update the animator based on movement
        animator.SetBool("isWalking", isMoving); // Set isWalking based on movement input
    }

    private void HandleJump()
    {
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Flipendo") || collision.gameObject.CompareTag("Spongify"))
        {
            isGrounded = true; // Set to true when touching the ground
        }

        if (gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
            rb.velocity = Vector3.zero;
        }
    }

}
