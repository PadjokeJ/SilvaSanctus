using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup masterMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup musicMixer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayAudio(AudioClip clip, Vector3 position, float volume)
    {
        PlayAudio(clip, position, volume, 0f);
    }

    public void PlayAudio(AudioClip clip, Vector3 position, float volume, float pitchVar)
    {
        GameObject audioSourceObject = new GameObject("audioSource");
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSourceObject.transform.position = position;

        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxMixer;

        audioSource.pitch = 1f + Random.Range(-pitchVar, pitchVar);

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSourceObject, clipLength + 1f / audioSource.pitch);
    }

}
