using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.18f;
    private float currentSpeed;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 velocity;
    public bool isGrounded;


    public Animator animator;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = speed;
    }

    void Update()
    {
        #region Movement

        isGrounded = controller.isGrounded;  // Check if on the ground

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small downward force to keep grounded
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        #endregion

        #region Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)  // Check for jump input and grounded status
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Calculate jump velocity
        }

        velocity.y += gravity * Time.deltaTime;  // Apply gravity over time
        controller.Move(velocity * Time.deltaTime);  // Move the character with the new velocity
        #endregion

        #region Animations
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isCasting", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isCasting", false);
        }

        bool isMoving = (vertical != 0 || horizontal != 0);
        animator.SetBool("isWalking", isMoving);

        bool isCasting = animator.GetBool("isCasting");
        #endregion
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Skurge"))
        {
            currentSpeed = 0.5f;
        }
        else if (hit.gameObject.CompareTag("Stairs"))
        {
            currentSpeed = speed + 2f;
        }
        else
        {
            currentSpeed = speed;
        }
    }


}
