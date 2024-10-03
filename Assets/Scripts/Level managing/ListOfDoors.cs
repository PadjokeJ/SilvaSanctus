using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListOfDoors : MonoBehaviour
{
    public bool dungeonDoor = true;

    public List<GameObject> doors;
    List<GameObject> listOfEnemies;

    public int enemyCount;

    List<EnemyAI> enemyList = new List<EnemyAI>();

    private void Awake()
    {
        listOfEnemies = new List<GameObject>();
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                listOfEnemies.Add(child.gameObject);
                child.gameObject.GetComponent<EnemyAI>().listOfDoors = this;
                enemyList.Add(child.gameObject.GetComponent<EnemyAI>());
            }
        }
        enemyCount = listOfEnemies.Count;

        InitDoors();
        DisableAllEnemies();
    }

    public void OnEnemyDeath()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            OpenDoors();
            if (dungeonDoor)
                BuffCards.instance.EnableCards();
        }
    }

    void DisableAllEnemies()
    {
        foreach (EnemyAI enemy in enemyList)
        {
            enemy.enabled = false;
        }

        if (WeaponManaging.hardMode)
        {
            foreach (EnemyAI enemy in enemyList)
            {
                Health eHlth = enemy.GetComponent<Health>();
                eHlth.health *= 2f;
                eHlth.maxHealth *= 2f;

                enemy.reactionTime *= 0.5f;
                enemy.experienceGiven *= 2f;
                enemy.maxDist = 20f;


            }
        }
    }

    void EnableAllEnemies()
    {
        foreach (EnemyAI enemy in enemyList)
        {
            if (enemy != null)
                enemy.enabled = true;
        }


        List<EnemyAI> eAICopy = new List<EnemyAI>(enemyList);

        foreach (EnemyAI enemy in eAICopy)
        {
            enemyList.Remove(enemy);
        }
    }


    void InitDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().lOD = this;
        }
    }

    public void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().Close();
        }

        EnableAllEnemies();
    }

    void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().Open();
        }
    }
}
