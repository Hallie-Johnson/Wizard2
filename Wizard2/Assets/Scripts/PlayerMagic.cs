using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{

    public Camera playerCamera;        // Reference to the player camera
    public GameObject particlePrefab;  // Particle effect prefab
    public Transform player;           // Reference to the player object
    public float rayDistance = 100f;   // Max distance for raycasting
    public float travelSpeed = 5f;     // Speed at which the particle travels

    private GameObject activeParticle = null; // Keeps track of the active particle above the player
    private bool isLookingAtSpell = false;    // Tracks if the player is looking at a target spell
    private RaycastHit hitSpellInfo;          // Stores the hit info for the spell target

    void Update()
    {
        // Cast a ray from the center of the camera's viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

        // Check if the left mouse button is being held
        if (Input.GetMouseButton(0))
        {
            // If there is no active particle, spawn one above the player
            if (activeParticle == null)
            {
                activeParticle = Instantiate(particlePrefab, player.position + Vector3.up * 2, Quaternion.identity);
            }
            else
            {
                // Keep the particle above the player while holding the button
                activeParticle.transform.position = player.position + Vector3.up * 2;
            }

            // Check for spell target
            isLookingAtSpell = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Rictusempra") ||
                    hit.collider.CompareTag("Skurge") ||
                    hit.collider.CompareTag("Diffindo") ||
                    hit.collider.CompareTag("Spongify") ||
                    hit.collider.CompareTag("Lumos") ||
                    hit.collider.CompareTag("Alohomora") ||
                    hit.collider.CompareTag("Flipendo"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))  // When the left mouse button is released
        {
            if (activeParticle != null)
            {
                if (isLookingAtSpell)
                {
                    // Move the particle to the target after releasing the button
                    StartCoroutine(MoveParticleToTarget(activeParticle, hitSpellInfo.point));
                }
                else
                {
                    // If not looking at a spell, destroy the particle
                    Destroy(activeParticle);
                }
                activeParticle = null;  // Reset after firing or destroying the particle
            }
        }
    }

    // Coroutine to move the particle to the target position and destroy it once it reaches
    private System.Collections.IEnumerator MoveParticleToTarget(GameObject particle, Vector3 target)
    {
        float maxTravelTime = 5f;  // Maximum time allowed for the particle to reach the target
        float elapsedTime = 0f;    // Tracks how long the particle has been moving

        // Loop until the particle reaches the target, the particle is destroyed, or the max time is reached
        while (particle != null && Vector3.Distance(particle.transform.position, target) > 0.01f && elapsedTime < maxTravelTime)
        {
            particle.transform.position = Vector3.MoveTowards(particle.transform.position, target, travelSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;  // Increment the time tracker
            yield return null;
        }

        // Destroy the particle if it still exists after reaching the target or exceeding the time limit
        if (particle != null)
        {
            Destroy(particle);
        }
    }




    /*
    public Camera playerCamera;        // Reference to the player camera
    public GameObject particlePrefab;  // Particle effect prefab
    public Transform player;           // Reference to the player object
    public float rayDistance = 100f;   // Max distance for raycasting
    public float travelSpeed = 5f;     // Speed at which the particle travels

    private GameObject spawnedParticle; // Reference to the spawned particle
    private bool isLookingAtSpell = false; // Tracks if the player is looking at "Skurge"
    private RaycastHit hitSpellInfo;  // Stores the hit info for the "Skurge" object

    void Update()
    {
        // Cast a ray from the center of the camera's viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

        // Check if the left mouse button is being held
        if (Input.GetMouseButton(0))
        {
            if (spawnedParticle == null)
            {
                // Spawn the particle above the player if it doesn't exist
                spawnedParticle = Instantiate(particlePrefab, player.position + Vector3.up * 2, Quaternion.identity);
            } else
            {
                // Update the particle's position to follow the player while the mouse button is held
                spawnedParticle.transform.position = player.position + Vector3.up * 2;
            }

            // Loop through the hits and check for "Skurge"
            isLookingAtSpell = false;
            foreach (RaycastHit hit in hits)
            {

                if (hit.collider.CompareTag("Rictusempra") ||
                hit.collider.CompareTag("Skurge") ||
                hit.collider.CompareTag("Diffindo") ||
                hit.collider.CompareTag("Spongify") ||
                hit.collider.CompareTag("Lumos") ||
                hit.collider.CompareTag("Alohomora") ||
                hit.collider.CompareTag("Flipendo"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (spawnedParticle != null)
            {
                if (isLookingAtSpell)
                {
                    // Start moving the particle toward the Skurge object
                    StartCoroutine(MoveParticleToTarget(spawnedParticle, hitSpellInfo.point));
                }
                else
                {
                    // Destroy the particle if not looking at Skurge
                    Destroy(spawnedParticle);
                }
            }
        }
    }

    // Coroutine to move the particle to the target position and destroy it once it reaches
    private System.Collections.IEnumerator MoveParticleToTarget(GameObject particle, Vector3 target)
    {
        // Loop until the particle reaches the target or the particle is destroyed
        while (particle != null && Vector3.Distance(particle.transform.position, target) > 0.5f)
        {
            particle.transform.position = Vector3.MoveTowards(particle.transform.position, target, travelSpeed * Time.deltaTime);
            yield return null;
        }

        // Check if the particle still exists before destroying it
        if (particle != null)
        {
            Destroy(particle);
        }
    } */
}