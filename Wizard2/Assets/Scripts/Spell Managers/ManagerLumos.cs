using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLumos : MonoBehaviour
{
    // Reference to the target object you want to modify
    public GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        // You can initialize or setup here if needed
    }

    // Update is called once per frame
    void Update()
    {
        // You can add update logic here if needed
    }

    // Function to make the target object translucent and remove its collider
    public void ActivateLumos()
    {
        // Check if the target object is assigned
        if (targetObject != null)
        {
            // Get the Renderer component of the target object
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Change material to Standard Shader if not already
                if (renderer.material.shader.name != "Standard")
                {
                    renderer.material.shader = Shader.Find("Standard");
                }

                // Set material to Transparent mode
                Material material = renderer.material;
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
            Debug.LogWarning("Target object is not assigned in ManagerLumos.");
        }
    }
}
