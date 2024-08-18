using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    EventSystem eventSystem;
    GameObject options;
    public GameObject menuButtons;

    bool hasDoneTutorial;
    void Start()
    {
        string filePath = Application.persistentDataPath + "/.tutorial";
        hasDoneTutorial = System.IO.File.Exists(filePath);
    }
    void Awake()
    {
        options = GetComponentInChildren<Options>().gameObject;
        options.SetActive(false);

        eventSystem = FindAnyObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        if (hasDoneTutorial)
            SceneManager.LoadScene(1);
        else
            Debug.Log("Player has not completed the tutorial");
    }
    public void Options()
    {
        options.SetActive(true);
        eventSystem.SetSelectedGameObject(options.GetComponentsInChildren<Button>()[0].gameObject);
        menuButtons.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void WentBackToThis()
    {
        eventSystem.SetSelectedGameObject(menuButtons.GetComponentInChildren<Button>().gameObject);
    }
}
