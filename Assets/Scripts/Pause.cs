using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused = false;
    RectTransform panelRect;
    [SerializeField] Vector2 pausedVector, playedVector;

    string selectedTab;

    public List<GameObject> tabs;
    List<string> tabNames;
    GameObject selectedTabObject;
    void Start()
    {
        panelRect = GameObject.Find("Pause Panel").GetComponent<RectTransform>();
        tabNames = new List<string>();
        foreach(GameObject obj in tabs)
        {
            tabNames.Add(obj.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, pausedVector, 0.2f);

        }
        else
            panelRect.anchoredPosition = Vector2.Lerp(panelRect.anchoredPosition, playedVector, 0.2f);
    }
    public void TogglePause()
    {
        selectedTabObject = tabs[0];
        ChangeTabs("Graphics");
        paused = !paused;
        if (paused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void ChangeTabs(string tab)
    {
        selectedTabObject.SetActive(false);
        selectedTabObject = tabs[tabNames.IndexOf(tab)];
        selectedTabObject.SetActive(true);
    }
}
