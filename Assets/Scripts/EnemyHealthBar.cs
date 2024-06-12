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

        //SpawnHealthBars();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for(int i = 0; i < positions.Count; i++)
        {
            healthBars[i].value = enemies[i].GetComponent<Health>().health;
            positions[i] = enemies[i].transform.position + new Vector3(0f, 1f, 0f);
            healthBarsObjects[i].transform.position = positions[i];
        }
        */
    }
    
    /*public List<GameObject> UpdateEnemyList() //also takes care of the HEALTH, POSITIONS
    {
        List<GameObject> listOfEnemies = new List<GameObject>();
        positions.Clear();
        health.Clear();
        maxHealth.Clear();
        foreach(GameObject gO in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Debug.Log("Indexed an enemy");
            listOfEnemies.Add(gO);
            positions.Add(gO.transform.position + new Vector3(0f, 1f, 0f));
            health.Add(gO.GetComponent<Health>().health);
            maxHealth.Add(gO.GetComponent<Health>().maxHealth);
        }
        return listOfEnemies;
    }
    
    public void SpawnHealthBars()
    {
        ClearHealthBars();
        enemies = UpdateEnemyList();
        Debug.Log(enemies.Count);
        foreach(GameObject enemy in enemies)
        {
            healthBarsObjects.Add(Instantiate<GameObject>(SliderPrefab, this.gameObject.transform));
        }
        int i = 0;
        Slider healthBarSlider;
        foreach(GameObject healthBar in healthBarsObjects)
        {
            healthBarSlider = healthBar.GetComponent<Slider>();
            healthBars.Add(healthBarSlider);
            healthBars[i].maxValue = enemies[i].GetComponent<Health>().health;
            i++;
        }
    }

    void ClearHealthBars()
    {
        healthBars.Clear();
        foreach (GameObject sliderObject in healthBarsObjects)
        {
            Destroy(sliderObject.transform);
        }
        healthBarsObjects.Clear();
    }*/

    public GameObject newHealthBar(EnemyAI enemy)
    {
        GameObject healthBarObject;

        healthBarObject = Instantiate<GameObject>(barPrefab, this.gameObject.transform);

        return healthBarObject;
    }

    public void updateHealthBar (GameObject healthBar, Vector3 enemyPos, float enemyHealth, float enemyMaxHealth)
    {
        healthBar.GetComponent<RectTransform>().position = enemyPos + new Vector3 (0, 1.5f, 0);
        
        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = enemyHealth / enemyMaxHealth;
    }
}
