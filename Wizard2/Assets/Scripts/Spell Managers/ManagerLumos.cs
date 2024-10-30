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
                // Change material to Standard Shader if not already
                if (renderer != null)
                {
                    if (renderer.material.shader.name != "Standard")
                    {
                        renderer.material.shader = Shader.Find("Standard");
                    }

                    // Set material to Transparent mode
                    material = renderer.material;
                    material.SetFloat("_Mode", 3); // Set mode to Transparent
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;

                    // Modify the material's color to make it 25% translucent
                    Color color = material.color;
                    color.a = 0.25f; // Set alpha to 25%
                    material.color = color;
                }

                // Remove the collider from the target object
                Collider collider = targetObject.GetComponent<Collider>();
                if (collider != null)
                {
                    Destroy(collider); // Destroy the collider component
                }
            }
            else
            {
                // If the toggle is off, reset to opaque and add a collider back
                if (renderer != null)
                {
                    // Ensure the material is set to opaque
                    material = renderer.material;
                    material.SetFloat("_Mode", 0); // Set mode to Opaque
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1; // Default render queue for opaque

                    // Reset material's color to fully opaque
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