using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C3StarPlatform : MonoBehaviour
{
    // Boolean values for directional movement
    public bool moveX = false;
    public bool moveY = false;
    public bool moveZ = false;

    // Reference to another GameObject
    public GameObject targetObject;

    // Distance to move the target object on the y-axis per press
    public float targetMoveDistanceY = -8.0f;

    // Speed of the cube and target movement
    public float moveSpeed = 1.0f;

    // Number of units to move the cube in specified direction
    public int moveNumber = 1;

    // Private flags for movement states
    private bool startMovement = false;
    private bool cubeReachedTarget = false;
    private bool targetReachedTarget = false;

    // Initial and target positions for smooth movement
    private Vector3 cubeStartPosition;
    private Vector3 cubeTargetPosition;
    private Vector3 targetObjectTargetPosition;

    private void Start()
    {
        // Set initial position for cube and calculate initial target position
        cubeStartPosition = transform.position;
        CalculateCubeTargetPosition();

        if (targetObject != null)
        {
            // Set the initial target position for the target object
            targetObjectTargetPosition = targetObject.transform.position + new Vector3(0, targetMoveDistanceY, 0);
        }
    }

    // Public method to start moving the cube and target object
    public void MoveCube()
    {
        gameObject.tag = "Untagged";

        startMovement = true;
        cubeReachedTarget = false;
        targetReachedTarget = false;

        // Calculate new target positions for cube and target object
        CalculateCubeTargetPosition();
        if (targetObject != null)
        {
            targetObjectTargetPosition = targetObject.transform.position + new Vector3(0, targetMoveDistanceY, 0);
        }
    }

    private void Update()
    {
        if (startMovement)
        {
            // Gradually move the cube to its target position
            transform.position = Vector3.MoveTowards(transform.position, cubeTargetPosition, moveSpeed * Time.deltaTime);
            if (transform.position == cubeTargetPosition) cubeReachedTarget = true;

            // Gradually move the target object downward each time
            if (targetObject != null)
            {
                targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetObjectTargetPosition, moveSpeed * Time.deltaTime);
                if (targetObject.transform.position == targetObjectTargetPosition) targetReachedTarget = true;
            }

            // Stop movement once both objects reach their target positions
            if (cubeReachedTarget && targetReachedTarget)
            {
                startMovement = false;
            }
        }
    }

    // Calculate the target position for the cube based on moveNumber and directional booleans
    private void CalculateCubeTargetPosition()
    {
        Vector3 moveDirection = Vector3.zero;
        if (moveX) moveDirection.x = moveNumber;
        if (moveY) moveDirection.y = moveNumber;
        if (moveZ) moveDirection.z = moveNumber;
        cubeTargetPosition = cubeStartPosition + moveDirection;
    }



    /*
    // Boolean values for directional movement
    public bool moveX = false;
    public bool moveY = false;
    public bool moveZ = false;

    // Reference to another GameObject
    public GameObject targetObject;

    // Distance to move the target object on the y-axis
    public float targetMoveDistanceY = -2.0f;

    // Speed of the movement
    public float moveSpeed = 1.0f;

    // Private flag to ensure the target object is only moved once
    private bool targetMoved = false;

    // Public method to start moving the cube and target object, can be called from another script
    public void MoveCube()
    {
        Vector3 moveDirection = Vector3.zero;

        if (moveX) moveDirection.x = 5;
        if (moveY) moveDirection.y = 5;
        if (moveZ) moveDirection.z = 5;

        // Move the cube in the specified direction
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Move the target object downward only once
        if (targetObject != null && !targetMoved)
        {
            Vector3 newPosition = targetObject.transform.position;
            newPosition.y += targetMoveDistanceY;
            targetObject.transform.position = newPosition;
            targetMoved = true;
        }
        gameObject.tag = "Untagged";
    } */
}
