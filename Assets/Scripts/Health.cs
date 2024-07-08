using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    string entityType;
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == this.gameObject)
        {
            entityType = "Player";
        }
        else if(this.gameObject.CompareTag("Enemy"))
        {
            entityType = "Enemy";
        }
        else if (this.gameObject.CompareTag("Chest"))
        {
            entityType = "Chest";
        }
        maxHealth = health;
    }
    public void takeDamage(float val)
    {
        health -= val;
        if(health <= 0)
        {
            if(entityType == "Player") GetComponent<PlayerManager>().Die();
            if(entityType == "Enemy") GetComponent<EnemyAI>().Die();
            if (entityType == "Chest") GetComponent<Chest>().GiveRewards();
        }
    }
    public void heal(float val)
    {
        health += val;
    }
}
