using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Health playerHealth;
    Image healthBar;
    void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerManager>().GetComponent<Health>();
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerHealth.health / playerHealth.maxHealth;
    }
}
