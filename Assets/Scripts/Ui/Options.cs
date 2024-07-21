using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        pause.WentBackToThis();
        

        this.gameObject.SetActive(false);
    }
}
