using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    string entityType;
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == this.gameObject)
        {
            entityType = "Player";
        }
        else
        {
            entityType = "Enemy";
        }
    }
    public void takeDamage(float val)
    {
        health -= val;
        if(health < 0)
        {
            if(entityType == "Player") GetComponent<PlayerManager>().Die();
            if(entityType == "Enemy") GetComponent<EnemyAI>().Die();
        }
    }
    public void heal(float val)
    {
        health += val;
    }
}
