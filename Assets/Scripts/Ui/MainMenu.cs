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
        SceneManager.LoadScene(1);
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
