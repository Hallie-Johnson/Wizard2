using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;
    public Rigidbody PlayerBody;
    private float Speed = 6f;
    private float JumpForce = 8f;
    public bool isGrounded;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public Animator animator;

    private Vector3 movementInput;
    private Vector3 movementVelocity;

    // New flag to disable movement during a launch
    [HideInInspector] public bool isLaunched = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        if (isLaunched) return; // Skip movement if launched

        // Handle input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        movementInput = (transform.forward * vertical + transform.right * horizontal).normalized;

        // Rotate player to face the camera direction
        if (movementInput.magnitude >= 0.1f)
        {
            float targetAngle = cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Handle animations
        HandleAnimations(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        if (isLaunched) return; // Skip movement if launched

        // Apply movement velocity
        movementVelocity = movementInput * Speed;
        Vector3 newVelocity = new Vector3(movementVelocity.x, PlayerBody.velocity.y, movementVelocity.z);
        PlayerBody.velocity = newVelocity;

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void HandleAnimations(float horizontal, float vertical)
    {
        bool isMoving = (vertical != 0 || horizontal != 0);
        animator.SetBool("isWalking", isMoving);

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isCasting", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isCasting", false);
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground") || hit.gameObject.CompareTag("Wall") || hit.gameObject.CompareTag("Flipendo") || hit.gameObject.CompareTag("Stairs"))
        {
            isGrounded = true;
            isLaunched = false; // Reset launch state on landing
        }
        
        if (hit.gameObject.CompareTag("Skurge"))
        {
            Speed = 0.5f;
        }
        else if (hit.gameObject.CompareTag("Stairs"))
        {
            Speed += 2f;
        }
        else
        {
            Speed = 6f;
        }
    }
}
