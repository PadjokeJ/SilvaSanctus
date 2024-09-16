using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Options : MonoBehaviour
{
    GameObject selectedTabObject;

    public GameObject previousTab;

    public AudioClip clickClip;

    public Sprite highlightedSprite, clickedSprite, selectedSprite, normalSprite;
    void Awake()
    {
        selectedTabObject = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        for (int i = 0; i < selectedTabObject.transform.childCount; i++)
        {
            Transform child = selectedTabObject.transform.GetChild(i);
            if (child.TryGetComponent<Toggle>(out Toggle toggle))
            {
                toggle.isOn = SaveManager.RetrieveBool(toggle.name);
                print(SaveManager.RetrieveBool(toggle.name));
            }
        }
    }

    public void ChangeTabs(GameObject tab)
    {
        selectedTabObject.transform.parent.GetComponent<Image>().sprite = normalSprite;
        selectedTabObject.SetActive(false);
        selectedTabObject = tab;
        selectedTabObject.transform.parent.GetComponent<Image>().sprite = selectedSprite;
        selectedTabObject.SetActive(true);

        PlayClickAudio();
    }

    void OnEnable()
    {
        selectedTabObject.SetActive(false);
        selectedTabObject = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        selectedTabObject.SetActive(true);
        selectedTabObject.transform.parent.GetComponent<Image>().sprite = selectedSprite;
    }

    public void GoBack()
    {
        PlayClickAudio();
        
        previousTab.SetActive(true);

        Pause pause = FindAnyObjectByType<Pause>();
        MainMenu mainMenu = FindAnyObjectByType<MainMenu>();
        if (pause != null)
            pause.WentBackToThis();
        if (mainMenu != null)
            mainMenu.WentBackToThis();
        
        this.gameObject.SetActive(false);
    }
    public void PPToggle(Toggle toggle)
    {
        PlayClickAudio();

        string name = toggle.name;
        bool state = toggle.isOn;
        Volume volume;
        volume = FindAnyObjectByType<Volume>();

        foreach (VolumeComponent volumeComp in volume.profile.components)
        {
            Debug.Log(volumeComp);
            if (volumeComp.name.StartsWith(name))
            {
                volumeComp.active = state;
                
                SaveManager.SaveBool(name, state);
                break;
            }
        }
    }


    void PlayClickAudio()
    {
        AudioManager.instance.PlayAudio(clickClip, Vector3.zero, 1f, 0.1f);
    }
}
