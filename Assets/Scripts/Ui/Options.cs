using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Options : MonoBehaviour
{
    GameObject selectedTabObject;

    public GameObject previousTab;
    void Awake()
    {
        selectedTabObject = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    public void ChangeTabs(GameObject tab)
    {
        selectedTabObject.SetActive(false);
        selectedTabObject = tab;
        selectedTabObject.SetActive(true);
    }

    void OnEnable()
    {
        ChangeTabs(transform.GetChild(0).GetChild(0).GetChild(1).gameObject);
    }

    public void GoBack()
    {
        
        previousTab.SetActive(true);

        Pause pause = FindAnyObjectByType<Pause>();
        MainMenu mainMenu = FindAnyObjectByType<MainMenu>();
        if (pause != null)
            pause.WentBackToThis();
        if (mainMenu != null)
            mainMenu.WentBackToThis();
        
        this.gameObject.SetActive(false);
    }

    public void OptionChange(string type, string name, int value)
    {
        if (type == "Graphics")
        {
            GraphicsChange(name, value);
        }
    }
    public void OptionChange(string type, string name, bool state)
    {
        /*if (type == "Graphics")
        {*/

            GraphicsChange(name, state);
        //}
    }
    public void OptionChange(string type, string name, float value)
    {
        if (type == "Graphics")
        {
            GraphicsChange(name, value);
        }
    }

    void GraphicsChange(string name, float value)
    {

    }
    void GraphicsChange(string name, int value)
    {

    }
    void GraphicsChange(string name, bool state)
    {
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

    public void Toggle(Toggle toggle)
    {
        OptionChange(toggle.transform.parent.name, toggle.name, toggle.isOn);
    }


    void RetrieveAllOptions()
    {

    }
}
