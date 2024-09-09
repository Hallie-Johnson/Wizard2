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

            // Destroy the particle immediately after triggering the shrink manager
            //Destroy(gameObject);
        }
    }


}
