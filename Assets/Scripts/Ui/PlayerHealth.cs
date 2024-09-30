using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PlayerHealth : MonoBehaviour
{
    Health playerHealth;
    Image healthBar;

    TextMeshProUGUI healthText;


    void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerManager>().GetComponent<Health>();
        healthBar = GetComponent<Image>();

        healthText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.CeilToInt(((0.64f * playerHealth.health / playerHealth.maxHealth) + .18f) * 16f) / 16f;
        healthText.text = $"{Mathf.Round(playerHealth.health * 100f) / 100f}/{Mathf.Round(playerHealth.maxHealth * 100f) / 100f}";
    }
}
