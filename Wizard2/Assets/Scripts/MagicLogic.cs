using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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
    }


}
