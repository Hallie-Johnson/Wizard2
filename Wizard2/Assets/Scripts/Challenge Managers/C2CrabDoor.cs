using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2CrabDoor : MonoBehaviour
{
    public GameObject firecrab1;      // First Firecrab object
    public GameObject firecrab2;      // Second Firecrab object
    public float moveUpAmount = 3.0f; // The amount to move the door up
    public float moveSpeed = 2.0f;    // Speed of the door's movement

    private bool isMoving = false;    // Flag to check if the door should move
    private Vector3 targetPosition;   // Target position for the door

    void Start()
    {
        // Set the target position by moving this door's initial position up
        targetPosition = transform.position + new Vector3(0, moveUpAmount, 0);
    }

    void Update()
    {
        // Check if both firecrabs are stunned
        if (!isMoving && AreBothFirecrabsStunned())
        {
            isMoving = true;  // Start moving the door
        }

        // Move the door if it needs to be moved
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving when the door reaches the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    // Helper method to check if both firecrabs are stunned
    private bool AreBothFirecrabsStunned()
    {
        // Get the ManagerRictusempra component from both firecrabs
        var manager1 = firecrab1.GetComponent<ManagerRictusempra>();
        var manager2 = firecrab2.GetComponent<ManagerRictusempra>();

        // Check if both managers exist and their isStunned properties are true
        return manager1 != null && manager2 != null && manager1.isStunned && manager2.isStunned;
    }
}
