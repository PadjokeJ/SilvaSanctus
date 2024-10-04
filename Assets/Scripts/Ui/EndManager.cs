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
        int level = startLevel;
        leftText.text = level.ToString();
        rightText.text = (level + 1).ToString();
        slider.minValue = PlayerLevelling.MaxExp(level - 2);
        slider.maxValue = PlayerLevelling.MaxExp(level - 1);
        slider.value = startExp;

        int leftLevel = startLevel;
        int rightLevel = startLevel + 1;
        float exp = startExp, maxExp = PlayerLevelling.experiencePoints;

        while (exp < maxExp)
        {
            slider.value = exp;
            exp = Mathf.Lerp(exp, maxExp + 1f, 0.025f);
            exp = Mathf.Min(exp, maxExp);

            yield return new WaitForSecondsRealtime(0.01f);

            if (exp > slider.maxValue)
            {
                leftLevel++;
                rightLevel++;
                leftText.text = leftLevel.ToString();
                rightText.text = rightLevel.ToString();

                slider.minValue = slider.maxValue;

                slider.maxValue = PlayerLevelling.MaxExp(rightLevel - 2);
            }

            Debug.Log($"{slider.minValue} -- [{slider.value}] -- {slider.maxValue}");
        }
        exp = maxExp;

        if (exp < slider.minValue)
        {
            leftLevel++;
            rightLevel++;
            leftText.text = leftLevel.ToString();
            rightText.text = rightLevel.ToString();

            slider.minValue = slider.maxValue;

            slider.maxValue = PlayerLevelling.MaxExp(rightLevel - 2);
        }

        slider.value = exp;
        
    }
}
