using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        gameOverReason.text = "You Win!";
    }

    public void LevelCleared()
    {
        panel.SetActive(true);
        gameOverReason.text = "Level Cleared!";
    }

    public void PlayerLoses()
    {
        panel.SetActive(true);
        gameOverReason.text = "Game Over!";
    }
}
