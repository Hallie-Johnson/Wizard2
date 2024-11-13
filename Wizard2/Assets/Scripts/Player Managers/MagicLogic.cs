using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLogic : MonoBehaviour
{
    public LayerMask wallLayer;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit tag: " + other.tag);

        if (other.CompareTag("Skurge"))
        {
            ManagerSkurge shrinkManager = other.GetComponent<ManagerSkurge>();
            if (shrinkManager != null)
            {
                shrinkManager.StartShrinking();
            }
        } 
        else if (other.CompareTag("Diffindo"))
        {
            ManagerDiffindo diffindoManager = other.GetComponent<ManagerDiffindo>();
            if (diffindoManager != null)
            {
                diffindoManager.ClearPath();
            }
        }
        else if (other.CompareTag("Rictusempra"))
        {
            ManagerRictusempra rictusempraManager = other.GetComponent<ManagerRictusempra>();
            if (rictusempraManager != null)
            {
                rictusempraManager.Stunned();
            }
        }
        else if (other.CompareTag("Spongify"))
        {
            ManagerSpongify spongifyManager = other.GetComponent<ManagerSpongify>();
            if (spongifyManager != null)
            {
                spongifyManager.EnableJumping();
            }
        }
        else if (other.CompareTag("Flipendo"))
        {
            ManagerFlipendo flipendoManager = other.GetComponent<ManagerFlipendo>();
            ManagerFlipendoButton flipendoButtonManager = other.GetComponent<ManagerFlipendoButton>();
            C3StarPlatform c3StarPlatform = other.GetComponent<C3StarPlatform>();
            if (flipendoManager != null)
            {
                // Calculate the direction from the object to the player
                Vector3 directionToPlayer = transform.position - other.transform.position;
                directionToPlayer.Normalize();  // Normalize to get the direction only

                // Determine which axis is the dominant one (X or Z) based on the player's position relative to the object
                Vector3 pushDirection = Vector3.zero;
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.z))
                {
                    // Player is to the left or right of the cube (dominant in X axis)
                    if (directionToPlayer.x > 0)
                    {
                        // Player is to the right, push the object to the left
                        pushDirection = Vector3.left; // Push to the left (-X)
                    }
                    else
                    {
                        // Player is to the left, push the object to the right
                        pushDirection = Vector3.right; // Push to the right (+X)
                    }
                }
                else
                {
                    // Player is in front or behind the cube (dominant in Z axis)
                    if (directionToPlayer.z > 0)
                    {
                        // Player is in front, push the object backward
                        pushDirection = Vector3.back; // Push backward (-Z)
                    }
                    else
                    {
                        // Player is behind, push the object forward
                        pushDirection = Vector3.forward; // Push forward (+Z)
                    }
                }

                // Get the collider of the object to push
                Collider collider = other.GetComponent<Collider>();
                if (collider == null)
                {
                    Debug.LogError("No collider found on the Flipendo object.");
                    return;
                }

                // Perform a raycast in the push direction to check for walls
                //float pushDistance = 1f;  // Adjust based on your needs
                float pushDistance = other.transform.localScale.x;

                // Lower the raycast origin by half the height of the collider or as needed
                //Vector3 raycastOrigin = other.transform.position + Vector3.up * (collider.bounds.extents.y / 2 - 0.75f); // Lowered by a bit more than half
                Vector3 raycastOrigin = other.transform.position + Vector3.up * ((collider.bounds.extents.y / 75) - collider.bounds.extents.y);

                // Visualize the raycast origin and direction
                Debug.DrawRay(raycastOrigin, pushDirection * pushDistance, Color.red, 2f); // The ray is drawn for 2 seconds

                RaycastHit hit;
                if (!Physics.Raycast(raycastOrigin, pushDirection, out hit, pushDistance, wallLayer))
                {
                    // No wall detected in the push direction, proceed with pushing
                    flipendoManager.StartPush(other.gameObject, pushDirection);
                }
                else
                {
                    // Wall detected in the push direction, do not push
                    Debug.Log("Wall detected in push direction. Push aborted.");
                }
            }

            if (flipendoButtonManager != null)
            {
                flipendoButtonManager.RotateObjects();
            }

            if (c3StarPlatform != null)
            {
                c3StarPlatform.MoveCube();
            }
        } 
        else if (other.CompareTag("Lumos"))
        {
            ManagerLumos lumosManager = other.GetComponent<ManagerLumos>();
            if (lumosManager != null)
            {
                lumosManager.ActivateLumos();
            }
        }

    }


}
