using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRictusempra : MonoBehaviour
{
    public Transform pointA;  // First end point
    public Transform pointB;  // Second end point
    private float moveSpeed = 1f;  // Speed at which the object moves
    private float rotationSpeed = 5f;  // Speed of rotation

    private Transform targetPoint;  // The current target end point
    private bool isTurning = false;
    private bool isStunned = false;

    void Start()
    {
        // Start moving toward pointA
        targetPoint = pointA;
    }

    void Update()
    {
        if (!isTurning && !isStunned)
        {
            // Move toward the target point
            MoveTowardsTarget();
        } 
        
        if (isStunned)
        {
            moveSpeed = 0;
        } else
        {
            moveSpeed = 1f;
        }
    }

    void MoveTowardsTarget()
    {
        // Move the object towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // Check if the object has reached the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Start turning around
            StartCoroutine(TurnAround());
        }
    }

    System.Collections.IEnumerator TurnAround()
    {
        isTurning = true;

        // Turn 180 degrees
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 180f, 0f));
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Switch to the other target point
        targetPoint = targetPoint == pointA ? pointB : pointA;

        isTurning = false;
    }

    // The Stunned function with a 5-second wait
    public void Stunned()
    {
        isStunned = true;  // Set isStunned to true
        StartCoroutine(WaitAndUnstun());  // Start the coroutine to wait and unstun
    }

    // Coroutine to wait 5 seconds and then unstun
    System.Collections.IEnumerator WaitAndUnstun()
    {
        yield return new WaitForSeconds(5f);  // Wait for 5 seconds
        isStunned = false;  // Set isStunned back to false
    }
}
