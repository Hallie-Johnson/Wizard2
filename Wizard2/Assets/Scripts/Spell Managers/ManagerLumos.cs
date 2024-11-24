using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLumos : MonoBehaviour
{
    public GameObject targetObject; // Assign the target object in the Inspector
    public bool shouldActivateLumos = true; // Toggle setting to activate or deactivate the effect

    public void ActivateLumos()
    {
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
                // Reset to opaque if toggle is off
                if (renderer != null)
                {
                    material = renderer.material;

                    // Set material to opaque
                    material.SetFloat("_Surface", 0); // Set Surface Type to Opaque
                    material.SetFloat("_AlphaClip", 1); // Enable alpha clipping
                    material.SetOverrideTag("RenderType", "Opaque");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

                    // Reset blending
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);

                    // Reset color to fully opaque
                    Color color = material.color;
                    color.a = 1.0f; // Set alpha to 100%
                    material.color = color;
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
