using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLumos : MonoBehaviour
{
    public GameObject targetObject; // Assign the target object in the Inspector
    public bool shouldActivateLumos = true; // Toggle setting to activate or deactivate the effect

    public GameObject[] targetObjects = new GameObject[4]; // Array to hold 4 GameObjects
    public Material lumosMaterial; // Material to apply to the target objects
    public Material lumosBlockMaterial; // Material to apply to the target objects
    public Animator animator; // Animator to control activation animations

    public void ActivateLumos()
    {
        gameObject.tag = "Untagged";

        if (lumosMaterial == null)
        {
            Debug.LogWarning("Lumos material is not assigned.");
            return;
        }

        // Apply the lumos material to each target object
        foreach (GameObject target in targetObjects)
        {
            if (target != null)
            {
                Renderer renderer = target.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = lumosMaterial; // Change the material
                }
                else
                {
                    Debug.LogWarning($"Renderer not found on {target.name}");
                }
            }
            else
            {
                Debug.LogWarning("One of the target objects is null.");
            }
        }

        // Activate the animator if assigned
        if (animator != null)
        {
            animator.SetBool("isActivated", true); // Set the activation boolean
        }
        else
        {
            Debug.LogWarning("Animator is not assigned.");
        }


        // Check if the target object is assigned
        if (targetObject != null)
        {
            // Get the Renderer component of the target object
            Renderer renderer = targetObject.GetComponent<Renderer>();
            Material material;

            if (shouldActivateLumos)
            {
                // Set up the material to be transparent
                if (renderer != null)
                {
                    material = renderer.material;

                    // Use URP/Lit shader if not already
                    if (material.shader.name != "Universal Render Pipeline/Lit")
                    {
                        material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    }

                    // Set material properties for transparency
                    material.SetFloat("_Surface", 1); // Set Surface Type to Transparent
                    material.SetFloat("_AlphaClip", 0); // Disable alpha clipping
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    // Adjust blending for transparency
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                    // Set the alpha value for semi-transparency
                    Color color = material.color;
                    color.a = 0.5f; // Set alpha to 50% (adjust as needed)
                    material.color = color;
                }

                // Optionally, remove the collider to make it pass-through
                Collider collider = targetObject.GetComponent<Collider>();
                if (collider != null)
                {
                    Destroy(collider);
                }
            }
            else
            {
                MeshRenderer meshRenderer = targetObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true; // Ensure the mesh is visible

                    // Set the material to the predefined one
                    if (lumosMaterial != null)
                    {
                        meshRenderer.material = lumosBlockMaterial;
                    }
                    else
                    {
                        Debug.LogWarning("Lumos material is not assigned.");
                    }
                }
                else
                {
                    Debug.LogWarning($"MeshRenderer not found on {targetObject.name}");
                }

                // Ensure there is a collider on the target object
                if (targetObject.GetComponent<Collider>() == null)
                {
                    targetObject.AddComponent<BoxCollider>(); // Add a BoxCollider or any other collider type you need
                }
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned in ManagerLumos.");
        }
    }
}
