using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverReason;
    GameObject panel;

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

}
