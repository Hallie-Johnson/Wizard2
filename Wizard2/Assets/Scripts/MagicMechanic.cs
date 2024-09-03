using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MagicMechanic : MonoBehaviour
{

    public CinemachineFreeLook freeLookCamera;  // Reference to the Cinemachine FreeLook camera
    public Transform sphere;  // Reference to the sphere
    public float inversionFactor = 1.0f;

    public GameObject particlePrefab;  // Reference to the particle system prefab
    private GameObject currentParticleSystem;  // Reference to the instantiated particle system


    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    void Update()
    {
        //Vector3 spherePosition = sphere.position;
        //spherePosition.y = freeLookCamera.transform.position.y;
        //sphere.position = spherePosition;

        // Get the Y position of the camera
        float cameraY = freeLookCamera.transform.position.y;

        // Invert the camera's Y position and apply an optional factor
        float invertedY = -cameraY * inversionFactor;

        // Update the sphere's position
        Vector3 spherePosition = sphere.position;
        spherePosition.y = invertedY + 5;
        sphere.position = spherePosition;


        // Check if the left mouse button is being held down
        if (Input.GetMouseButton(0))  // 0 is the left mouse button
        {
            // If there isn't already an instance of the particle system, create one
            if (currentParticleSystem == null)
            {
                currentParticleSystem = Instantiate(particlePrefab, sphere.position, Quaternion.identity);
                // Optionally set the particle system to play automatically upon instantiation
                var particleSystem = currentParticleSystem.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
            else
            {
                // Update the position of the existing particle system
                currentParticleSystem.transform.position = sphere.position;
            }
        }
        else
        {
            // Destroy the particle system when the left mouse button is released
            if (currentParticleSystem != null)
            {
                Destroy(currentParticleSystem);
            }
        }



    }
}
