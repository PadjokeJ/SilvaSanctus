using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    List<GameObject> enemies;
    List<Vector3> positions;
    List<float> health;
    List<float> maxHealth;
    List<Slider> healthBars;
    List<GameObject> healthBarsObjects;

    public GameObject SliderPrefab;
    public GameObject barPrefab;
    void Awake()
    {
        enemies = new List<GameObject>();
        

        positions = new List<Vector3>();
        health = new List<float>();
        maxHealth = new List<float>();
        healthBars = new List<Slider>();
        healthBarsObjects = new List<GameObject>();

    }

    public GameObject newHealthBar(EnemyAI enemy)
    {
        GameObject healthBarObject;

        healthBarObject = Instantiate<GameObject>(barPrefab, this.gameObject.transform);

        return healthBarObject;
    }

    public void updateHealthBar (GameObject healthBar, Vector3 enemyPos, float enemyHealth, float enemyMaxHealth)
    {
        healthBar.GetComponent<RectTransform>().position = enemyPos + new Vector3 (0, 1.4f, 0);
        
        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = enemyHealth / enemyMaxHealth;

        healthBar.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, enemyHealth / enemyMaxHealth);
    }
}
