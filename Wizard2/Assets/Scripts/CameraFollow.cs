using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Desired position is the player position plus the offset
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Rotate the camera to always look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust to look at the player's head height
    }

}
