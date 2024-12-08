using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRictusempra : MonoBehaviour
{
    public Transform pointA;  // First end point
    public Transform pointB;  // Second end point
    public Transform player;  // Reference to the player
    public Animator crabAnimator;  // Reference to the crab's Animator

    private float moveSpeed = 1f;  // Speed at which the object moves
    private float rotationSpeed = 5f;  // Speed of rotation
    private Transform targetPoint;  // The current target end point
    private bool isTurning = false;
    public bool isStunned = false;
    private bool playerSeen = false;  // Boolean to track if the player is seen
    private bool isShooting = false;  // Track if the crab is already shooting


    public GameObject projectilePrefab;  // The sphere projectile prefab
    public Transform firePoint;  // The point from where the crab shoots the projectile
    public float shootForce = 10f;  // The base force for the projectile
    public float detectionRange = 10f;  // Distance at which the crab sees the player
    public float arcHeight = 10f;  // Height of the arc for the projectile
    private float shootingInterval = 1.9f;  // Interval between shots in seconds

    public AudioClip fireballSound;
    private AudioSource oneShotAudioSource;

    void Start()
    {
        targetPoint = pointA;
        oneShotAudioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = crabAnimator.GetCurrentAnimatorStateInfo(0);  // 0 is the layer index
        if (stateInfo.IsName("get_up") || stateInfo.IsName("stunned"))
        {
            // Set the crab's y position to match pointA's y position when stunned
            Vector3 newPosition = transform.position;
            newPosition.y = pointA.position.y - 0.5f;  // Set the y value to pointA's y position
            transform.position = newPosition;  // Apply the new position

            // If stunned, stop shooting and reset related flags
            if (isShooting)
            {
                StopCoroutine(ShootAtIntervals());
                isShooting = false;  // Ensure it stops shooting while stunned
            }
        }
        else
        {
            Vector3 newPosition = transform.position;
            newPosition.y = pointA.position.y;  // Reset the y value to the original height
            transform.position = newPosition;

            // Check the distance to the player
            CheckPlayerDistance();

            if (!isStunned)  // Only continue actions if not stunned
            {
                if (!playerSeen && !isTurning)
                {
                    // Continue moving between points A and B
                    MoveTowardsTarget();
                }
                else if (playerSeen && !isShooting)  // Only start shooting if not already shooting and not stunned
                {
                    // Face the player
                    FacePlayer();
                    isShooting = true;  // Set the flag before starting the coroutine
                    StartCoroutine(ShootAtIntervals());  // Only start once
                }
                else if (playerSeen)
                {
                    FacePlayer();
                }
            }
        }

        if (isStunned)
        {
            crabAnimator.SetBool("isStunned", true);
            moveSpeed = 0;
            //gameObject.tag = "Flipendo";
            gameObject.tag = "Untagged";
        }
        else
        {
            crabAnimator.SetBool("isStunned", false);
            moveSpeed = 1f;
            gameObject.tag = "Rictusempra";
        }
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = targetPoint.position - transform.position;
        directionToTarget.y = 0;  // Keep the crab level on the Y-axis

        // Rotate towards the target smoothly
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // Check if the crab has reached the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            StartCoroutine(TurnAround());
        }
    }

    System.Collections.IEnumerator TurnAround()
    {
        isTurning = true;

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 180f, 0f));
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        targetPoint = targetPoint == pointA ? pointB : pointA;

        isTurning = false;
    }

    // Coroutine to wait 5 seconds and then unstun
    System.Collections.IEnumerator WaitAndUnstun()
    {
        yield return new WaitForSeconds(5f);
        isStunned = false;
    }

    // Check the distance between the fire crab and the player
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !playerSeen)
        {
            playerSeen = true;
            crabAnimator.SetBool("PlayerSeen", true);  // Activate the noticed animation
        }
        else if (distanceToPlayer > detectionRange && playerSeen)
        {
            playerSeen = false;
            crabAnimator.SetBool("PlayerSeen", false);  // Reset the animation
        }
    }

    // Rotate the crab to continuously face the player
    void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;  // Ensure the crab stays level while rotating

        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // The Stunned function with a 5-second wait
    public void Stunned()
    {
        isStunned = true;
        StartCoroutine(WaitAndUnstun());
    }

    System.Collections.IEnumerator ShootAtIntervals()
    {
        while (playerSeen && !isStunned)  // Ensure the crab doesn't shoot while stunned
        {
            ShootProjectileAtPlayer();
            yield return new WaitForSeconds(shootingInterval);  // Wait for the interval before shooting again
        }
        isShooting = false;  // Reset the shooting flag when done
    }

    void ShootProjectileAtPlayer()
    {
        oneShotAudioSource.PlayOneShot(fireballSound);
        // Double-check if the crab is in a state where it can shoot
        AnimatorStateInfo stateInfo = crabAnimator.GetCurrentAnimatorStateInfo(0);  // 0 is the layer index
        if (stateInfo.IsName("get_up") || stateInfo.IsName("stunned"))
        {
            return;
        }

        // Spawn the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Calculate direction and force to create an arc
        Vector3 direction = (player.position - firePoint.position).normalized;
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);

        // Apply horizontal force and additional vertical force to create an arc
        Vector3 force = horizontalDirection * shootForce + Vector3.up * arcHeight;
        rb.AddForce(force, ForceMode.Impulse);
    }
}

