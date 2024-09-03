using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 250;
    public float jumpForce = 5f; // Force applied when jumping
    private bool isGrounded = true; // To check if the player is on the ground
    private Rigidbody rb;

    private bool canMove = true;
    private bool canMoveW = true;
    private bool canMoveA = true;
    private bool canMoveS = true;
    private bool canMoveD = true;

    public Camera freeLookCamera;
    public float rotationSmoothTime = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {



        // Rotate the player based on mouse movement
        /*float mouseHorizontal = Input.GetAxis("Mouse X");
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - mouseHorizontal * turnSpeed * Time.deltaTime, 0);
        rb.MoveRotation(targetRotation); */

        if (canMove)
        {


            // Move the player forward in the direction it is facing
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movementV = transform.forward * moveVertical * speed * Time.deltaTime;
            if (canMoveW && moveVertical > 0) rb.MovePosition(rb.position + movementV);
            else if (canMoveS && moveVertical < 0) rb.MovePosition(rb.position + movementV);

            // Move the player horizontally (strafing)
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 movementH = transform.right * moveHorizontal * speed * Time.deltaTime;
            if (canMoveA && moveHorizontal < 0) rb.MovePosition(rb.position + movementH);
            else if (canMoveD && moveHorizontal > 0) rb.MovePosition(rb.position + movementH);

            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }


    }

    // Check if the player is grounded by detecting collisions with the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set to true when touching the ground
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;

            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 playerPosition = transform.position;

            Vector3 directionToContact = (contactPoint - playerPosition).normalized;

            // Determine which side of the player is colliding with the obstacle
            float forwardDot = Vector3.Dot(transform.forward, directionToContact);
            float rightDot = Vector3.Dot(transform.right, directionToContact);

            if (forwardDot > 0.5f) canMoveW = false;  // Front
            if (forwardDot < -0.5f) canMoveS = false; // Back
            if (rightDot > 0.5f) canMoveD = false; // Right
            if (rightDot < -0.5f) canMoveA = false; // Left
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Allow movement again after leaving the obstacle
            canMoveW = true;
            canMoveA = true;
            canMoveS = true;
            canMoveD = true;
        }
    }

    private void LateUpdate()
    {
        if (rb == null || freeLookCamera == null) return;

        // Get the camera's Y rotation
        float cameraYRotation = freeLookCamera.transform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraYRotation, 0);

        // Smoothly rotate the player's Rigidbody
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothTime));
    }

}

