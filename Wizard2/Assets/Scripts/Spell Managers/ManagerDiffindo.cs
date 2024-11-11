using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerDiffindo : MonoBehaviour
{
    public C3RopePuzzle c3RopePuzzle;

    public void ClearPath()
    {
        gameObject.SetActive(false);

        if (gameObject.name.Contains("Rope"))
        {
            //Debug.Log(gameObject.name + " contains 'Rope'. Calling OnRopeDestroyed...");

            // Call the OnRopeDestroyed function from the RopeOrderCheck script
            c3RopePuzzle.OnRopeDestroyed(gameObject);
        }
    }
}
