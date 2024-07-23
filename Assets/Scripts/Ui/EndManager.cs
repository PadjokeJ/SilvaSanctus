using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverReason;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerWins()
    {
        gameOverReason.text = "You Win!";
    }

    public void PlayerLoses()
    {
        gameOverReason.text = "Game Over!";
    }
}
