using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject canvasPrefab;
    public GameObject healthBarPrefab;

    Image healthBar;

    BossR bossR;

    private void Awake()
    {
        maxHealth = health;

        bossR = GetComponent<BossR>();
    }

    public void SpawnHealthCanvas()
    {
        GameObject healthBarCanvas = Instantiate<GameObject>(canvasPrefab);
        GameObject healthBarObject = Instantiate<GameObject>(healthBarPrefab, healthBarCanvas.transform);

        healthBar = healthBarObject.transform.GetChild(0).GetComponent<Image>();

        healthBar.fillAmount = 0f;

        Debug.Log(healthBar.fillAmount);

        StartCoroutine(LerpHealthValue());
    }

    public void ChangeHealthBar()
    {
        StopAllCoroutines();

        if (health <= 0)
        {
            bossR.Die();
        }

        StartCoroutine(LerpHealthValue());
    }
    IEnumerator LerpHealthValue()
    {
        for (int i = 0; i <= 50f; i++)
        {
            Debug.Log(healthBar.fillAmount);
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        healthBar.fillAmount = health / maxHealth;
    }
}
