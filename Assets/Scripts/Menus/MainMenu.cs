using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton;
    public GameObject optionsCanvas;

    public void Start()
    {
        playButton.GetComponent<Button>().Select();
        AudioManager.instance.PlaySound("EVA");
    }

    public void Options()
    {
        optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);
    }

    public void PlayGame()
    {
        AudioManager.instance.StopSound("EVA");

        GameObject.FindObjectOfType<LevelLoader>().LoadNextLevel("ControlsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
