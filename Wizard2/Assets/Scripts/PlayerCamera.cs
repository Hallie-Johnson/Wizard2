using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform player; // The player's transform
    public Vector3 offset; // Offset behind the player
    public float sensitivity = 100f; // How sensitive the camera rotation is
    public float distance = 3f; // Default camera distance from the player
    public float minDistance = 1f; // Minimum distance from player to prevent clipping
    public float minY = -30f, maxY = 60f; // Vertical camera rotation limits
    public float cameraSmoothTime = 0.1f; // Smoothing for camera movement
    public float rotationSmoothTime = 0.1f; // Smoothing for player rotation

    private float currentX = 0f, currentY = 0f;
    private Vector3 cameraVelocity; // For smooth camera movement
    private float smoothVelocity; // For smooth player rotation

    void LateUpdate()
    {
        // Rotate based on mouse input
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Clamp vertical rotation (up/down)
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // Calculate desired camera position
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // Check for ground collision to adjust camera position
        RaycastHit hit;
        if (Physics.Raycast(player.position, desiredPosition - player.position, out hit, distance))
        {
            desiredPosition = hit.point;
        }

        // Prevent camera from getting too close
        float currentDistance = Vector3.Distance(player.position, desiredPosition);
        if (currentDistance < minDistance)
        {
            desiredPosition = player.position + rotation * offset.normalized * minDistance;
        }

        // Smoothly move the camera to the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraVelocity, cameraSmoothTime);

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * offset.y);

        // Smooth player rotation
        float desiredPlayerRotationY = transform.eulerAngles.y;
        float smoothedRotationY = Mathf.SmoothDampAngle(player.eulerAngles.y, desiredPlayerRotationY, ref smoothVelocity, rotationSmoothTime);
        player.rotation = Quaternion.Euler(0f, smoothedRotationY, 0f);
    }


}
