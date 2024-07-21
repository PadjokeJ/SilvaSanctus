using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused = false;
    RectTransform panelRect;
    [SerializeField] Vector2 pausedVector, playedVector;
    void Start()
    {
        panelRect = GameObject.Find("Pause Panel").GetComponent<RectTransform>();
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
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
