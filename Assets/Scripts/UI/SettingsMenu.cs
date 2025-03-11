using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown resolutionDropdown;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    AudioMixer audioMixer;


    Resolution[] resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRateRatio.numerator == 60).ToArray();
        resolutionDropdown.ClearOptions();
        List<string> resolutionStr = new List<string>();
        foreach (Resolution resolution in resolutions) resolutionStr.Add(resolution.width.ToString() + " X " + resolution.height.ToString());
        resolutionDropdown.AddOptions(resolutionStr);
        resolutionDropdown.value = resolutionDropdown.options.Count - 1;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void updateFoV(float value)
    {
        if (mainCamera == null) return;
        mainCamera.fieldOfView = value;
    }

    public void setGraphicPreset(int preset)
    {
        QualitySettings.SetQualityLevel(preset);
    }

    public void setFullScreenMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                break;
        }
    }

    public void setResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreenMode == FullScreenMode.FullScreenWindow);
    }
    public void setMouseSensitivity(float sens)
    {
        if (playerController == null) return;
        playerController.sensitivity = sens;
    }

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume - 80); // -80 because mixer uses scale from -80 to 20
    }

    public void setSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume - 80);
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume - 80);
    }

}
