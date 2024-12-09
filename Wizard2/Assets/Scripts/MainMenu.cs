using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip UISound; // Drag your audio clip here
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.LoadSceneAsync(6);
    }

    public void ResetGame()
    {
        GameManager.Instance.c1_grade = "N/A";
        GameManager.Instance.c2_grade = "N/A";
        GameManager.Instance.c3_grade = "N/A";
        GameManager.Instance.c4_grade = "N/A";
        SceneManager.LoadSceneAsync(0);
    }
}
