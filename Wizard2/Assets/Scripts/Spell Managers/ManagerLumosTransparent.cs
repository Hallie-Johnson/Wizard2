using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLumosTransparent : MonoBehaviour
{
    void Start()
    {
        // Ensure the Renderer is attached to the GameObject
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a new material with the Standard shader
            Material material = new Material(Shader.Find("Standard"));

            // Set material to Transparent mode
            material.SetFloat("_Mode", 3); // Set mode to Transparent
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            // Set the material's color to be transparent
            Color color = material.color;
            color.a = 0.25f; // Set alpha to 25%
            material.color = color;

            // Assign the new material to the renderer
            renderer.material = material;
        }

        // Remove any collider component
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider); // Destroy the collider component
        }
    }
}
