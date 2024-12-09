using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Global data variables
    public string c1_grade = "N/A";
    public string c2_grade = "N/A";
    public string c3_grade = "N/A";
    public string c4_grade = "N/A";

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist this object between scenes
    }
}
