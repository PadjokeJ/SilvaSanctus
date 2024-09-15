using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListOfDoors : MonoBehaviour
{
    public bool dungeonDoor = true;

    public List<GameObject> doors;
    List<GameObject> listOfEnemies;

    int enemyCount;

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

    void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().Open();
        }
    }
}
