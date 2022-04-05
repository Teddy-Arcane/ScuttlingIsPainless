using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public GameObject resumeButton;
    
    private ControllerMapping controls;
    private InputAction pauseAction;

    public GameObject pauseMenuUI;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        controls = new ControllerMapping();

        pauseAction = controls.Player.Pause;
        pauseAction.started += ctx => ButtonHit();
        pauseAction.Enable();
    }

    private void ButtonHit()
    {
        if (paused)
            Resume();
        else
            Pause();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        
        resumeButton.GetComponent<Button>().Select();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
