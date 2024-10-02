using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverReason;
    GameObject panel;

    public Slider slider;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    void Awake()
    {
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
    }

    public void PlayerWins()
    {
        ActivateEndScreen();
        Time.timeScale = 0f;
        gameOverReason.text = "You Win!";
    }

    public void LevelCleared()
    {
        ActivateEndScreen();
        Time.timeScale = 0f;
        gameOverReason.text = "Level Cleared!";
    }

    public void PlayerLoses()
    {
        ActivateEndScreen();
        Time.timeScale = 0f;
        gameOverReason.text = "Game Over!";
    }

    public void Restart()
    {
        StartCoroutine(SwitchScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void MainMenu()
    {
        StartCoroutine(SwitchScene(0));
    }

    void ActivateEndScreen()
    {
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        panel.SetActive(true);
        StartCoroutine(Glide(panel.GetComponent<RectTransform>(), new Vector2(0, 0)));

        StartCoroutine(GainLevel(PlayerLevelling.levelAtStartOfRun, PlayerLevelling.GetLevel()));
    }

    IEnumerator Glide(RectTransform panelTransform, Vector2 desiredPosition)
    {
        for (int i = 0; i < 20; i++)
        {
            panelTransform.anchoredPosition = Vector2.Lerp(panelTransform.anchoredPosition, desiredPosition, 0.5f);
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }

    IEnumerator SwitchScene(int index)
    {
        FindAnyObjectByType<Transition>().FadeToBlack();
        yield return new WaitForSecondsRealtime(0.6f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    IEnumerator GainLevel(int startLevel, int endLevel)
    {
        float startExp = PlayerLevelling.expAtStartOfRun;
        float endExp = startExp;
        int level = 0;
        if (startLevel == endLevel)
            level = endLevel;
        else
            level = startLevel;
        while (level <= endLevel)
        {
            if (level != startLevel)
                startExp = PlayerLevelling.MaxExp(level - 1);
            endExp = PlayerLevelling.MaxExp(level);
            if (level == endLevel)
                endExp = PlayerLevelling.experiencePoints;
            leftText.text = level.ToString();
            rightText.text = (level + 1).ToString();
            StartCoroutine(LerpLevel(startExp, endExp, PlayerLevelling.MaxExp(level - 1), PlayerLevelling.MaxExp(level)));
            yield return new WaitForSecondsRealtime(0.51f);
            level += 1;
        }
    }
    IEnumerator LerpLevel(float startExp, float endExp, float minExp, float maxExp)
    {
        slider.maxValue = maxExp;
        slider.minValue = minExp;
        float value = startExp;
        float minValue = value;
        float maxValue = endExp;
        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            value = Mathf.Lerp(minValue, maxValue, i / 50f);
            slider.value = value;
        }
    }

    
}
