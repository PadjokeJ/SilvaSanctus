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

    private void Awake()
    {
        maxHealth = health;

        GameObject healthBarCanvas = Instantiate<GameObject>(canvasPrefab);
        GameObject healthBarObject = Instantiate<GameObject>(healthBarPrefab, healthBarCanvas.transform);

        healthBar = healthBarObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void ChangeHealthBar()
    {
        //StopAllCoroutines();
        StartCoroutine(LerpHealthValue());
    }
    IEnumerator LerpHealthValue()
    {
        for (int i = 0; i <= 20f; i++)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, 0.9f);
            yield return new WaitForSeconds(0.01f);
        }
        healthBar.fillAmount = health / maxHealth;
    }
}
