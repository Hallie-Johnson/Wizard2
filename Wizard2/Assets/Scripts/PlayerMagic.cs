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
            }

            // Loop through the hits and check for "Skurge"
            isLookingAtSpell = false;
            foreach (RaycastHit hit in hits)
            {

                if (hit.collider.CompareTag("Rictusempra"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Skurge"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Diffindo"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Spongify"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Lumos"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Alohomora"))
                {
                    isLookingAtSpell = true;
                    hitSpellInfo = hit;
                    break;
                }

                if (hit.collider.CompareTag("Flipendo"))
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
        while (Vector3.Distance(particle.transform.position, target) > 0.1f)
        {
            particle.transform.position = Vector3.MoveTowards(particle.transform.position, target, travelSpeed * Time.deltaTime);
            yield return null;
        }

        // Destroy the particle after reaching the target
        Destroy(particle);
    }
}
