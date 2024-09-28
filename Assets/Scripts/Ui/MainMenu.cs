using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
    EventSystem eventSystem;
    GameObject options;
    public GameObject menuButtons;

    RectTransform menuButtonsTransform;
    Vector2 hiddenButtonsPos;
    public Vector2 currentPos;

    bool hasDoneTutorial;

    Transition transition;

    public AudioClip clickClip;

    public Slider levelSlider;
    public TextMeshProUGUI levelTMP;
    void Start()
    {
        string filePath = Application.persistentDataPath + "/.tutorial";
        hasDoneTutorial = System.IO.File.Exists(filePath);

        hiddenButtonsPos = new Vector2(-600, 0);
        menuButtonsTransform = menuButtons.GetComponent<RectTransform>();
    }
    void Awake()
    {
        transition = FindAnyObjectByType<Transition>();
        //transition.GetComponent<Image>().color = Color.black;
        transition.FadeToWhite();

        options = GetComponentInChildren<Options>().gameObject;
        options.SetActive(false);

        eventSystem = FindAnyObjectByType<EventSystem>();

        SetLevelSlider();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        menuButtonsTransform.anchoredPosition = Vector2.Lerp(menuButtonsTransform.anchoredPosition, currentPos, 0.5f);

        if (menuButtonsTransform.anchoredPosition.x < -590)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Play()
    {
        PlayClickAudio();

        if (hasDoneTutorial)
        {
            WeaponManaging wpmgr = FindAnyObjectByType<WeaponManaging>();
            currentPos = hiddenButtonsPos;
            wpmgr.hidden = !wpmgr.hidden;
            eventSystem.SetSelectedGameObject(wpmgr.firstSquare);
        }
        else
        {
            transition.FadeToBlack();
            StartCoroutine(DelayedSceneChange(0.6f, 1));
        }
    }
    public void Options()
    {
        PlayClickAudio();
        options.SetActive(true);
        eventSystem.SetSelectedGameObject(options.GetComponentsInChildren<Button>()[0].gameObject);
        currentPos = hiddenButtonsPos;
    }
    public void Quit()
    {
        PlayClickAudio();
        Application.Quit();
    }
    public void WentBackToThis()
    {
        eventSystem.SetSelectedGameObject(menuButtons.GetComponentInChildren<Button>().gameObject);
        currentPos = Vector2.zero;
    }

    void PlayClickAudio()
    {
        AudioManager.instance.PlayAudio(clickClip, Vector3.zero, 1f, 0.1f);
    }

    IEnumerator DelayedSceneChange(float timeDelay, int sceneIndex)
    {
        Debug.Log("started waiting");
        yield return new WaitForSecondsRealtime(timeDelay);
        Debug.Log("finished waiting");
        SceneManager.LoadScene(sceneIndex);
    }

    void SetLevelSlider()
    {
        int level = PlayerLevelling.GetLevel();
        levelTMP.text = "Level : " + level.ToString();

        levelSlider.minValue = PlayerLevelling.MaxExp(level - 1);
        levelSlider.maxValue = PlayerLevelling.MaxExp(level);
        levelSlider.value = SaveManager.RetrieveFloat("experience");
    }
}
