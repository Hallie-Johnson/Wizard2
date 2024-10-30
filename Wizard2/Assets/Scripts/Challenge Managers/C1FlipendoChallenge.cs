using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1FlipendoChallenge : MonoBehaviour
{
    public GameObject[] cubes; // Array to hold the cube objects (9 cubes)
    public GameObject[] doors; // Array to hold the door objects (3 doors)

    public float moveSpeed = 2.0f; // Speed at which doors move
    public float moveDistanceDoor1 = 6.0f; // Distance for door 1
    public float moveDistanceDoor2 = 5.0f; // Distance for door 2
    public float moveDistanceDoor3 = 5.0f; // Distance for door 3

    private Vector3 door1TargetPos;
    private Vector3 door2TargetPos;
    private Vector3 door3TargetPos;

    private Vector3 door1TargetPosDown;
    private Vector3 door2TargetPosDown;
    private Vector3 door3TargetPosDown;

    private void Start()
    {
        // Initialize target positions for the doors
        door1TargetPos = doors[0].transform.position + Vector3.up * moveDistanceDoor1;
        door2TargetPos = doors[1].transform.position + Vector3.up * moveDistanceDoor2;
        door3TargetPos = doors[2].transform.position + Vector3.up * moveDistanceDoor3;

        door1TargetPosDown = doors[0].transform.position + Vector3.down * (moveDistanceDoor1 - moveDistanceDoor1);
        door2TargetPosDown = doors[1].transform.position + Vector3.down * (moveDistanceDoor2 - moveDistanceDoor2);
        door3TargetPosDown = doors[2].transform.position + Vector3.down * (moveDistanceDoor3 - moveDistanceDoor3);

    }

    void Update()
    {
        MoveDoors();
    }

    private void MoveDoors()
    {
        // Check conditions for door 1
        if (cubes[0].transform.rotation.eulerAngles.x == 0 &&
            cubes[1].transform.rotation.eulerAngles.x == 270 &&
            cubes[2].transform.rotation.eulerAngles.x == 90)
        {
            Debug.Log("here1");
            StartCoroutine(MoveDoor(doors[0], door1TargetPos));
        }
        else if (doors[0].transform.position.y > 4) // Only move down if y > 1
        {
            StartCoroutine(MoveDoor(doors[0], door1TargetPosDown));
        }

        // Check conditions for door 2
        if (cubes[3].transform.rotation.eulerAngles.z == 0 &&
            cubes[4].transform.rotation.eulerAngles.z == 270 &&
            cubes[5].transform.rotation.eulerAngles.z == 180)
        {
            Debug.Log("here2");
            StartCoroutine(MoveDoor(doors[1], door2TargetPos));
        }
        else if (doors[1].transform.position.y > 3) // Only move down if y > 1
        {
            StartCoroutine(MoveDoor(doors[1], door2TargetPosDown));
        }

        // Check conditions for door 3
        if (cubes[6].transform.rotation.eulerAngles.z == 0 &&
            cubes[7].transform.rotation.eulerAngles.z == 180 &&
            cubes[8].transform.rotation.eulerAngles.z == 90)
        {
            Debug.Log("here3");
            StartCoroutine(MoveDoor(doors[2], door3TargetPos));
        }
        else if (doors[2].transform.position.y > 3) // Only move down if y > 1
        {
            StartCoroutine(MoveDoor(doors[2], door3TargetPosDown));
        }
    }

    private System.Collections.IEnumerator MoveDoor(GameObject door, Vector3 targetPos)
    {
        Vector3 startPos = door.transform.position;
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;

        while (Vector3.Distance(door.transform.position, targetPos) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            door.transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            yield return null; // Wait until the next frame
        }

        // Ensure the door reaches the target position exactly
        door.transform.position = targetPos;
    }
}
