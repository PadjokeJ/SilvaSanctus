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
        else if (this.gameObject.TryGetComponent<WaterBarrel>(out WaterBarrel waterBarrel))
        {
            entityType = "WaterBarrel";
        }
        else if (this.gameObject.TryGetComponent<Dummy>(out Dummy dummy))
        {
            entityType = "Dummy";
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
        else
        {
            health -= val;
        }

        if (entityType == "Dummy") GetComponent<Dummy>().Hurt();

        if (health <= 0)
        {
            if(entityType == "Player") GetComponent<PlayerManager>().Die();
            if(entityType == "Enemy") GetComponent<EnemyAI>().Die();
            if (entityType == "Chest") GetComponent<Chest>().GiveRewards();
            if (entityType == "WaterBarrel") GetComponent<WaterBarrel>().Die();
            
        }
    }
    public void heal(float val)
    {
        health += val;
        if (health > maxHealth)
            health = maxHealth;
    }
}
