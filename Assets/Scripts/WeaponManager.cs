using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    GameObject player;
    public float damage;
    public float reloadTime;
    public float distFromPlayer;
    public bool isMelee;
    public float knockBack;
    public ParticleSystem particle;

    TrailRenderer tr;

    Collider2D c2d;
    PlayerAttack pA;
    GenericWeaponManager gWP;
    List<GameObject> targets = new List<GameObject>();

    Animation attackAnimation;

    public UnityEvent attackEvent;
    void Start()
    {
        c2d = GetComponent<Collider2D>();
        pA = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        gWP = GetComponent<GenericWeaponManager>();
        gWP.attackEvent = attackEvent;
        gWP.reloadTime = reloadTime;
        gWP.weaponDamage = damage;
        gWP.knockback = knockBack;
        gWP.weaponDistance = distFromPlayer;

        player = FindObjectOfType<PlayerAttack>().gameObject;
        tr = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.emitting = gWP.isAttacking;
    }
    
    public void Attack()
    {
        targets.Clear();
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D item in hit)
        {
            if (item.CompareTag("Enemy"))
                targets.Add(item.gameObject);

        }
        gWP.targets = targets;

        foreach(GameObject target in targets)
        {
            target.GetComponent<Health>().takeDamage(damage);
            target.GetComponent<EnemyAI>().takeKB(player.transform.position, knockBack);
        }

        if (attackAnimation != null) attackAnimation.Play();
    }
}
