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

    private void Awake()
    {
        listOfEnemies = new List<GameObject>();
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                listOfEnemies.Add(child.gameObject);
                child.gameObject.GetComponent<EnemyAI>().listOfDoors = this;
            }
        }
        enemyCount = listOfEnemies.Count;

        InitDoors();
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
    }

    void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().Open();
        }
    }
}
