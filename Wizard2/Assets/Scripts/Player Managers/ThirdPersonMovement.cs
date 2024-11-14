using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public Transform cam;
    private Vector3 PlayerMovementInput;
    public Rigidbody PlayerBody;
    public float Speed = 3f;
    public float Jumpforce = 5f;
    private float currentSpeed;
    public bool isGrounded;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Animator animator;
    private Vector3 movementVelocity;
    private Vector3 smoothVelocity;

    // New flag to disable movement during a launch
    [HideInInspector] public bool isLaunched = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = Speed;
    }

    private void Update()
    {
        if (isLaunched) return; // Skip movement if launched

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //PlayerMovementInput = new Vector3(horizontal, 0f, vertical);
        Vector3 targetMovement = (transform.forward * vertical + transform.right * horizontal) * Speed;
        movementVelocity = Vector3.SmoothDamp(movementVelocity, targetMovement, ref smoothVelocity, 0.1f);
        PlayerBody.MovePosition(PlayerBody.position + movementVelocity * Time.fixedDeltaTime);

        if (targetMovement.magnitude > 0 || Input.GetMouseButton(0))
        {
            float targetAngle = cam.eulerAngles.y; // Face exactly where the camera is pointing
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }


        MovePlayer();

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

    private void MovePlayer()
    {
        //Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        //PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerBody.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
            isGrounded = false;
        }

        
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground") || hit.gameObject.CompareTag("Wall") ||
            hit.gameObject.CompareTag("Flipendo") || hit.gameObject.CompareTag("Spowngify"))
        {
            isGrounded = true;
            isLaunched = false; // Reset launch state on landing
        }
        else if (hit.gameObject.CompareTag("Skurge"))
        {
            currentSpeed = 0.5f;
        }
        else if (hit.gameObject.CompareTag("Stairs"))
        {
            currentSpeed = Speed + 2f;
        }
        else
        {
            currentSpeed = Speed;
        }
    } 
}
