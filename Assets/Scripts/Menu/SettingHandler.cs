using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

[System.Serializable]
public class SettingHandler : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider MusicSlider;

    private void Awake()
    {
        if (!(PlayerPrefs.HasKey("SFXVolume") && PlayerPrefs.HasKey("MusicVolume")))
        {
            SetSFXVolume(1.0f);
            SetMusicVolume(1.0f);
        }

        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void SetSFXVolume(float v)
    {
        PlayerPrefs.SetFloat("SFXVolume", v);
    }
    public void SetMusicVolume(float v)
    {
        PlayerPrefs.SetFloat("MusicVolume", v);
    }
}
