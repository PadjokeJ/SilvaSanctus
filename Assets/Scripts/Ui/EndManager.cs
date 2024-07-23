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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerWins()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameOverReason.text = "You Win!";
    }

    public void LevelCleared()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameOverReason.text = "Level Cleared!";
    }

    public void PlayerLoses()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameOverReason.text = "Game Over!";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
