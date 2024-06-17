using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public float damage;
    public float reloadTime;
    public float distFromPlayer;
    public bool isMelee;
    public float knockBack;
    public ParticleSystem particle;

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gWP.targets = targets;
        pA.targets = targets;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) targets.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player")) targets.Remove(collision.gameObject);
    }
    public void Attack()
    {
        attackAnimation.Play();
    }
}
