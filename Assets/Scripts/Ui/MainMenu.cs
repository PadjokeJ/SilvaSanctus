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

    RectTransform menuButtonsTransform;
    Vector2 hiddenButtonsPos;
    public Vector2 currentPos;

    bool hasDoneTutorial;
    void Start()
    {
        string filePath = Application.persistentDataPath + "/.tutorial";
        hasDoneTutorial = System.IO.File.Exists(filePath);

        hiddenButtonsPos = new Vector2(-600, 0);
        menuButtonsTransform = menuButtons.GetComponent<RectTransform>();
    }
    void Awake()
    {
        options = GetComponentInChildren<Options>().gameObject;
        options.SetActive(false);

        eventSystem = FindAnyObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        menuButtonsTransform.anchoredPosition = Vector2.Lerp(menuButtonsTransform.anchoredPosition, currentPos, 0.5f);
    }
    public void Play()
    {
        if (hasDoneTutorial)
        {
            WeaponManaging wpmgr = FindAnyObjectByType<WeaponManaging>();
            currentPos = hiddenButtonsPos;
            wpmgr.hidden = !wpmgr.hidden;
            eventSystem.SetSelectedGameObject(wpmgr.firstSquare);
        }
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
