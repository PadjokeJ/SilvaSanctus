using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool paused = false;
    RectTransform panelRect;
    [SerializeField] Vector2 pausedVector, playedVector;

    public EventSystem eventSystem;

    GameObject options;

    public List<GameObject> buttons;
    void Start()
    {
        panelRect = GameObject.Find("Pause Panel").GetComponent<RectTransform>();

        options = GetComponentInChildren<Options>().gameObject;
        options.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, pausedVector, 0.2f);
        else
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, playedVector, 0.2f);
    }
    public void TogglePause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
            eventSystem.SetSelectedGameObject(buttons[0].gameObject);
        }
        else
            Time.timeScale = 1f;
    }

    public void Options()
    {
        options.SetActive(true);
        eventSystem.SetSelectedGameObject(options.GetComponentsInChildren<Button>()[0].gameObject);
        panelRect.gameObject.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void WentBackToThis()
    {
        eventSystem.SetSelectedGameObject(buttons[0].GetComponent<Button>().gameObject);
    }
}
