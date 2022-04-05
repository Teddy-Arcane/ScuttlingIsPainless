using Assets.Scripts.NameGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsController : MonoBehaviour
{
    public TMP_Text textInput;

    public ControllerMapping mapping;
    private InputAction next;

    private void OnEnable()
    {
        mapping = new ControllerMapping();

        next = mapping.Player.Jump;
        next.started += ctx => Play();
        next.Enable();

        AudioManager.instance.PlaySound("EVA");

        textInput.text =
            $"{NameGenerator.instance.KillCount} defenceless souls expired while you repaired the ship.{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}Thank you for playing.{Environment.NewLine}{Environment.NewLine}Made with <3, by ArcaneRituals.{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}Press <<Jump>> to close the game.";
    }

    private void OnDisable()
    {
        next.Disable();
    }

    private void Play()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
