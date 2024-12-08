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

    private Vector3 smoothVelocity;

    public bool movement;
    private bool dead;

    public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioClip victorySound;
    public AudioClip cardSound;
    public AudioClip castingSound;
    public AudioClip castedSound;
    public AudioClip hitSound;
    private AudioSource loopAudioSource;
    private AudioSource oneShotAudioSource;


    // New flag to disable movement during a launch
    public bool isLaunched = false;

    void Start()
    {
        movement = true;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        loopAudioSource = gameObject.AddComponent<AudioSource>();
        oneShotAudioSource = gameObject.AddComponent<AudioSource>();

        // Configure loopAudioSource
        loopAudioSource.loop = true;
    }

    private void Update()
    {
        if (isLaunched) return; // Skip movement if launched

        if (dead)
        {
            movement = false;
        }

        if (movement)
        {
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

            HandleWalkingSound(horizontal, vertical);

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                PlayerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            HandleCastingSound();


        } else
        {
            loopAudioSource.enabled = false;
            return;
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

    private void HandleWalkingSound(float horizontal, float vertical)
    {
        bool isMoving = (horizontal != 0 || vertical != 0);

        if (isMoving && isGrounded)
        {
            if (loopAudioSource.clip != walkSound || !loopAudioSource.isPlaying)
            {
                loopAudioSource.clip = walkSound;
                loopAudioSource.loop = true;
                loopAudioSource.Play();
            }
        }
        else if (loopAudioSource.clip == walkSound)
        {
            loopAudioSource.Stop();
        }
    }

    private void HandleCastingSound()
    {
        if (Input.GetMouseButton(0))
        {
            if (loopAudioSource != castingSound || !loopAudioSource.isPlaying)
            {
                loopAudioSource.clip = castingSound;
                loopAudioSource.loop = true;
                loopAudioSource.Play();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (loopAudioSource.clip == castingSound)
            {
                loopAudioSource.Stop();
            }

            oneShotAudioSource.PlayOneShot(castedSound);
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
            oneShotAudioSource.PlayOneShot(hitSound);
        }
        else if (hit.gameObject.CompareTag("Stairs"))
        {
            Speed += 2f;
        }
        else
        {
            Speed = 6f;
        }

        if (hit.gameObject.CompareTag("SpongifyTarget"))
        {
               // Debug.Log("Player is on top of a SpongifyTarget!");

        }

        if (hit.gameObject.CompareTag("Death"))
        {
            dead = true;
            animator.SetTrigger("isFalling");
            oneShotAudioSource.PlayOneShot(deathSound);
        }

        if (hit.gameObject.CompareTag("Fireball"))
        {
            oneShotAudioSource.PlayOneShot(hitSound);
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            movement = false;
            animator.SetTrigger("isVictory");
            oneShotAudioSource.PlayOneShot(victorySound);
        } else if (other.gameObject.CompareTag("Card"))
        {
            oneShotAudioSource.PlayOneShot(cardSound);
        }
    }


}
