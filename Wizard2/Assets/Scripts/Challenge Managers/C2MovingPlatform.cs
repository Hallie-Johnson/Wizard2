using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2MovingPlatform : MonoBehaviour
{
    public GameObject skurge;      // Reference to the Skurge object
    public GameObject cube1;       // Reference to Cube 1
    public GameObject cube2;       // Reference to Cube 2
    public float moveDistance = 10.0f; // Distance each cube moves along the Z-axis
    public float moveSpeed = 2.0f; // Speed of the movement
    public float pauseDuration = 1.0f; // Duration to pause at each end of the movement

    private Vector3 cube1StartPos; // Starting position of Cube 1
    private Vector3 cube2StartPos; // Starting position of Cube 2
    private bool shouldMove = false; // Flag to start movement when Skurge is destroyed

    void Start()
    {
        // Save the starting positions of each cube
        cube1StartPos = cube1.transform.position;
        cube2StartPos = cube2.transform.position;
    }

    void Update()
    {
        // Check if Skurge object exists
        if (skurge == null && !shouldMove)
        {
            shouldMove = true; // Start movement when Skurge is destroyed
            StartCoroutine(MoveCubes());
        }
    }

    // Coroutine to move the cubes with a pause at each end
    private IEnumerator MoveCubes()
    {
        Vector3 cube1TargetPos = cube1StartPos + new Vector3(0, 0, moveDistance);
        Vector3 cube2TargetPos = cube2StartPos + new Vector3(0, 0, -moveDistance);

        while (shouldMove)
        {
            // Move Cube 1 to its target position
            yield return StartCoroutine(MoveToPosition(cube1, cube1TargetPos));
            // Move Cube 2 to its target position
            yield return StartCoroutine(MoveToPosition(cube2, cube2TargetPos));

            // Pause at the target position
            yield return new WaitForSeconds(pauseDuration);

            // Move Cube 1 back to its start position
            yield return StartCoroutine(MoveToPosition(cube1, cube1StartPos));
            // Move Cube 2 back to its start position
            yield return StartCoroutine(MoveToPosition(cube2, cube2StartPos));

            // Pause at the start position
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    // Coroutine to move an object to a target position
    private IEnumerator MoveToPosition(GameObject cube, Vector3 targetPos)
    {
        while (Vector3.Distance(cube.transform.position, targetPos) > 0.01f)
        {
            cube.transform.position = Vector3.MoveTowards(cube.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        cube.transform.position = targetPos; // Snap to target position to prevent drift
    }
}
