using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    
    void Awake()
    {
        float masterVolume;
        if (SaveManager.HasKey("MasterVolume"))
        {
            masterVolume = SaveManager.RetrieveFloat("MasterVolume");
            UpdateMasterVolume(masterVolume);
            masterSlider.value = masterVolume;
        }
        float musicVolume;
        if (SaveManager.HasKey("MusicVolume"))
        {
            musicVolume = SaveManager.RetrieveFloat("MusicVolume");
            UpdateMusicVolume(musicVolume);
            masterSlider.value = musicVolume;
        }
        float sfxVolume;
        if (SaveManager.HasKey("SFXVolume"))
        {
            sfxVolume = SaveManager.RetrieveFloat("SFXVolume");
            UpdateSFXVolume(sfxVolume);
            masterSlider.value = sfxVolume;
        }
    }

    public void UpdateMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
        SaveManager.SaveFloat("MasterVolume", value);
    }
    public void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        SaveManager.SaveFloat("MusicVolume", value);
    }

    public void UpdateSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        SaveManager.SaveFloat("SFXVolume", value);
    }
}
