using UnityEngine;
using System.Collections.Generic;

public class SpawnSpellIcons : MonoBehaviour
{
    // This script is used for the spell icons to spawn

    public Camera mainCamera;  // Reference to the main camera
    public GameObject player;  // Reference to the player object
    public GameObject cubePrefab;  // Reference to the cube prefab
    public float maxRayDistance = 100f;  // Max distance for raycasting
    public float surfaceOffset = 0.01f;  // Small offset to prevent the cube from being inside the object

    // List of materials corresponding to each tag
    public Material rictusempraMaterial;
    public Material skurgeMaterial;
    public Material diffindoMaterial;
    public Material spongifyMaterial;
    public Material lumosMaterial;
    public Material alohomoraMaterial;
    public Material flipendoMaterial;

    private GameObject spawnedCube;

    // Dictionary to map tags to materials
    private Dictionary<string, Material> tagToMaterial;

    void Start()
    {
        // Initialize the dictionary with tag-material pairs
        tagToMaterial = new Dictionary<string, Material>
        {
            { "Rictusempra", rictusempraMaterial },
            { "Skurge", skurgeMaterial },
            { "Diffindo", diffindoMaterial },
            { "Spongify", spongifyMaterial },
            { "Lumos", lumosMaterial },
            { "Alohomora", alohomoraMaterial },
            { "Flipendo", flipendoMaterial }
        };
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DetectObstacle();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (spawnedCube != null)
            {
                Destroy(spawnedCube);
            }
        }
    }

    void DetectObstacle()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Get all objects hit by the ray
        RaycastHit[] hits = Physics.RaycastAll(ray, maxRayDistance);

        foreach (RaycastHit hit in hits)
        {
            // Check if the object's tag matches any of the tags we are interested in
            if (tagToMaterial.ContainsKey(hit.collider.tag))
            {
                if (spawnedCube == null)  // If no cube has been spawned yet
                {
                    SpawnCube(hit, hit.collider.tag);
                }
                else
                {
                    UpdateCubePosition(hit);  // Update cube position if it's already spawned
                }
                return;  // Stop after the first valid hit
            }
        }

        // If no obstacle is hit, destroy the cube
        if (spawnedCube != null)
        {
            Destroy(spawnedCube);
        }
    }

    void SpawnCube(RaycastHit hit, string tag)
    {
        // Calculate the exact position where the cube should overlay the object
        Vector3 spawnPosition = hit.point + hit.normal * surfaceOffset;

        // Instantiate the cube at the calculated position
        spawnedCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        // Align the cube to face the player
        AlignCubeWithPlayer();

        // Assign the corresponding material based on the tag
        AssignMaterial(tag);
    }

    private Vector3 velocity = Vector3.zero;

    void UpdateCubePosition(RaycastHit hit)
    {
        // Target position where the cube should overlay the object
        Vector3 targetPosition = hit.point + hit.normal * surfaceOffset;

        // Smoothly move the cube to the target position using SmoothDamp
        spawnedCube.transform.position = Vector3.SmoothDamp(spawnedCube.transform.position, targetPosition, ref velocity, 0.1f);

        // Align the cube to face the player
        AlignCubeWithPlayer();
    }



    void AlignCubeWithPlayer()
    {
        // Make the cube face the player by setting its forward direction opposite the player's forward direction
        spawnedCube.transform.forward = -player.transform.forward;
    }

    void AssignMaterial(string tag)
    {
        // Assign the corresponding material from the dictionary based on the tag
        Material materialToAssign;
        if (tagToMaterial.TryGetValue(tag, out materialToAssign))
        {
            Renderer cubeRenderer = spawnedCube.GetComponent<Renderer>();
            cubeRenderer.material = materialToAssign;
        }
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic2 : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    public GameObject player;  // Reference to the player object
    public GameObject cubePrefab;  // Reference to the cube prefab
    public float maxRayDistance = 100f;  // Max distance for raycasting
    private float surfaceOffset = 0.5f;  // Small offset to prevent the cube from being inside the object

    private GameObject spawnedCube;

    public List<Material> materials;
    private Material material;

    void Update()
    {
        DetectObstacle();
    }

    void DetectObstacle()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Get all objects hit by the ray
        RaycastHit[] hits = Physics.RaycastAll(ray, maxRayDistance);

        foreach (RaycastHit hit in hits)
        {
            // Ignore objects tagged as "Ground" or "Player"
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Player"))
            {
                continue;  // Skip to the next object in the raycast
            }

            // If we hit an object with the tag "obstacle", proceed
            if (hit.collider.CompareTag("Rictusempra") ||
                    hit.collider.CompareTag("Skurge") ||
                    hit.collider.CompareTag("Diffindo") ||
                    hit.collider.CompareTag("Spongify") ||
                    hit.collider.CompareTag("Lumos") ||
                    hit.collider.CompareTag("Alohomora") ||
                    hit.collider.CompareTag("Flipendo"))
            {
                if (spawnedCube == null)  // If no cube has been spawned yet
                {
                    SpawnCube(hit);
                }
                else
                {
                    UpdateCubePosition(hit);  // Update cube position if it's already spawned
                }
                return;  // Stop after the first valid obstacle hit
            }
        }

        // If no obstacle is hit, destroy the cube
        if (spawnedCube != null)
        {
            Destroy(spawnedCube);
        }
    }

    void SpawnCube(RaycastHit hit)
    {
        // Calculate the exact position where the cube should overlay the object
        Vector3 spawnPosition = hit.point + hit.normal * surfaceOffset;

        // Instantiate the cube at the calculated position
        spawnedCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        // Align the cube to face the player
        AlignCubeWithPlayer();
    }

    void UpdateCubePosition(RaycastHit hit)
    {
        // Update the cube position to overlay the object
        spawnedCube.transform.position = hit.point + hit.normal * surfaceOffset;

        // Align the cube to face the player
        AlignCubeWithPlayer();
    }

    void AlignCubeWithPlayer()
    {
        // Make the cube face the player by setting its forward direction opposite the player's forward direction
        spawnedCube.transform.forward = -player.transform.forward;
    }
}
*/