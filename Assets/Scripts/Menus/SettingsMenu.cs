using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;

    public Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        var curResIndex = 0;
        var resStrings = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            var res = resolutions[i];
            var option = $"{res.width} x {res.height}";
            resStrings.Add(option);

            if (res.height == Screen.currentResolution.height 
                && res.width == Screen.currentResolution.width)
                curResIndex = i;
        }

        resolutionDropdown.AddOptions(resStrings);
        resolutionDropdown.value = curResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int index)
    {
        var res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
