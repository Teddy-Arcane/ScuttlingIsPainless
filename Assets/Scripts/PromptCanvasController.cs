using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptCanvasController : MonoBehaviour
{
    public static PromptCanvasController instance;

    public TMP_Text display;

    public GameObject canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetText(string text, bool showingName)
    {
        NameGenerator.instance.isShowingName = showingName;
        display.text = text;
    }

    public void ToggleCanvas(bool active)
    {
        canvas.SetActive(active);
    }
}
