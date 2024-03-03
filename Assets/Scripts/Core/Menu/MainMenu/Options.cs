using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle musicToggle;
    Resolution[] resolutions;

    private void Awake()
    {
        float volume = PlayerPrefs.GetFloat("SavedVolume");
        audioMixer.SetFloat("MasterVolume", volume);
        if(PlayerPrefs.GetInt("ToggleValue") == 1)
        {
            musicToggle.isOn = true;
        }
        else
        {
            musicToggle.isOn = false;
        }
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + " Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("SavedVolume", volume);
    }

    public void ToggleMusic()
    {
        if (musicToggle.isOn)
        {
            audioMixer.SetFloat("MusicVolume", 0f);
            PlayerPrefs.SetInt("ToggleValue", 1);

        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80f);
            PlayerPrefs.SetInt("ToggleValue", 0);
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
