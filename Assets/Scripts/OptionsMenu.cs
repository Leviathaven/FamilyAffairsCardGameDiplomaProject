using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject MainMenu;
    public GameObject OptionsMenuObject;
    public GameObject DropdownGraphics;
    public GameObject ToggleSoundOnOff;
    public GameObject SliderVolume;

    private void Awake()
    {
        MainMenu = GameObject.Find("MainMenu");
        OptionsMenuObject = GameObject.Find("OptionsMenu");
        DropdownGraphics = GameObject.Find("DropdownGraphics");
        ToggleSoundOnOff = GameObject.Find("ToggleSoundOnOff");
        SliderVolume = GameObject.Find("SliderVolume");
        LoadPrefs();
        OptionsMenuObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
    }

    public void SetSound()
    {
        AudioListener.pause = !ToggleSoundOnOff.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("SoundPaused", AudioListener.pause ? 1 : 0);
    }

    public void SetSound(bool state)
    {
        AudioListener.pause = state;
        PlayerPrefs.SetInt("SoundPaused", Convert.ToInt32(state));
    }

    public void SavePrefs()
    {
        PlayerPrefs.Save();
        OptionsMenuObject.SetActive(false);
        MainMenu.SetActive(true);
    }


    public void LoadPrefs()
    {
        var volume = PlayerPrefs.GetFloat("Volume", 1);
        var qualityIndex = PlayerPrefs.GetInt("QualityIndex", 0);
        var isPaused = PlayerPrefs.GetInt("SoundPaused", 0);

        var soundBool = Convert.ToBoolean(isPaused);

        SliderVolume.GetComponent<Slider>().value = volume;
        ToggleSoundOnOff.GetComponent<Toggle>().isOn = !soundBool;
        DropdownGraphics.GetComponent<Dropdown>().value = qualityIndex;

        SetVolume(volume);
        SetQuality(qualityIndex);
        SetSound(soundBool);
    }
}
