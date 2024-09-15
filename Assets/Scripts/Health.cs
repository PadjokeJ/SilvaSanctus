using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    string entityType;

    PlayerHurtAnimation pHA;

    public static Health playerInstance;
    void Awake()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            entityType = "Player";
            Health.playerInstance = this;
            pHA = FindObjectOfType<PlayerHurtAnimation>();
            
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
        if (entityType == "Enemy")
            health -= val * (1 + Buffs.damageBuff);
        if (entityType == "Player")
        {
            pHA.HurtScreen();
            health -= val * (1 - Buffs.defense);
        }
        if (health <= 0)
        {
            if(entityType == "Player") GetComponent<PlayerManager>().Die();
            if(entityType == "Enemy") GetComponent<EnemyAI>().Die();
            if (entityType == "Chest") GetComponent<Chest>().GiveRewards();
        }
    }
    public void heal(float val)
    {
        health += val;
        if (health > maxHealth)
            health = maxHealth;
    }
}
