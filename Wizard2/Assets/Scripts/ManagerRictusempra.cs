using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ManagerRictusempra : MonoBehaviour
{
    public Transform pointA;  // First end point
    public Transform pointB;  // Second end point
    public Transform player;  // Reference to the player
    public Animator crabAnimator;  // Reference to the crab's Animator
    private float detectionRange = 10f;  // Distance at which the crab sees the player

    private float moveSpeed = 1f;  // Speed at which the object moves
    private float rotationSpeed = 5f;  // Speed of rotation
    private Transform targetPoint;  // The current target end point
    private bool isTurning = false;
    private bool isStunned = false;
    private bool playerSeen = false;  // Boolean to track if the player is seen

    void Start()
    {
        targetPoint = pointA;
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
        } else
        {
            Vector3 newPosition = transform.position;
            newPosition.y = pointA.position.y;  // Reset the y value to the original height
            transform.position = newPosition;

            // Check the distance to the player
            CheckPlayerDistance();

            if (!playerSeen && !isTurning)
            {
                // Continue moving between points A and B
                MoveTowardsTarget();
            }
            else if (playerSeen)
            {
                // Face the player
                FacePlayer();
            }
        }

        if (isStunned)
        {
            crabAnimator.SetBool("isStunned", true);
            moveSpeed = 0;
            gameObject.tag = "Flipendo";
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
}
