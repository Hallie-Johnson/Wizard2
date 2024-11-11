using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C3RopePuzzle : MonoBehaviour
{
    // Rope GameObjects
    public GameObject rope1;
    public GameObject rope2;
    public GameObject rope3;
    public GameObject rope4;

    // Door to be raised
    public GameObject door;

    // Movement settings for raising the door
    public float doorRaiseDistance = 4.0f;
    public float doorRaiseSpeed = 1.0f;

    // Correct order of rope destruction
    private List<GameObject> correctOrder;
    private List<GameObject> destructionOrder;

    // Door movement control
    private bool raiseDoor = false;
    private Vector3 doorTargetPosition;

    private void Start()
    {
        // Define the correct order
        correctOrder = new List<GameObject> { rope3, rope1, rope2, rope4 };
        destructionOrder = new List<GameObject>();

        // Set initial door target position
        if (door != null)
        {
            doorTargetPosition = door.transform.position + new Vector3(0, doorRaiseDistance, 0);
            //Debug.Log("Door target position set to: " + doorTargetPosition);
        }

        //Debug.Log("Rope order initialized. Correct order: rope3, rope1, rope2, rope4.");
    }

    public void OnRopeDestroyed(GameObject rope)
    {
        // Add the destroyed rope to the order list
        destructionOrder.Add(rope);
        //Debug.Log("Rope destroyed: " + rope.name + ". Current destruction order: " + GetDestructionOrderNames());

        // Check if all ropes have been destroyed
        if (destructionOrder.Count == correctOrder.Count)
        {
            // Verify if the order is correct
            if (IsCorrectOrder())
            {
                //Debug.Log("Correct order achieved! Starting to raise the door.");
                raiseDoor = true;
            }
            else
            {
                //Debug.Log("Incorrect destruction order. Respawning ropes and resetting.");
                RespawnRopes();
                destructionOrder.Clear();
            }
        }
    }

    private bool IsCorrectOrder()
    {
        // Check if the destruction order matches the correct order
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (destructionOrder[i] != correctOrder[i])
            {
                //Debug.Log("Destruction order mismatch at index " + i + ": Expected " + correctOrder[i].name + ", but got " + destructionOrder[i].name);
                return false;
            }
        }
        //Debug.Log("Destruction order matches the correct order.");
        return true;
    }

    private void RespawnRopes()
    {
        // Re-enable the ropes (or respawn if needed)
        rope1.SetActive(true);
        rope2.SetActive(true);
        rope3.SetActive(true);
        rope4.SetActive(true);

        //Debug.Log("Ropes have been respawned and are ready to try again.");
    }

    private void Update()
    {
        // Gradually raise the door if the correct order was achieved
        if (raiseDoor && door != null)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorTargetPosition, doorRaiseSpeed * Time.deltaTime);
            //Debug.Log("Raising the door. Current position: " + door.transform.position);

            // Stop raising once the target height is reached
            if (door.transform.position == doorTargetPosition)
            {
                //Debug.Log("Door has reached the target position.");
                raiseDoor = false;
            }
        }
    }

    // Helper function to print the names of the destroyed ropes in the current order
    private string GetDestructionOrderNames()
    {
        List<string> ropeNames = new List<string>();
        foreach (GameObject rope in destructionOrder)
        {
            ropeNames.Add(rope.name);
        }
        return string.Join(", ", ropeNames);
    }
}
