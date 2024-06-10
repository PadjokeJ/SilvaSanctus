using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlashAttack : MonoBehaviour
{
    GenericWeaponManager gwp;
    bool isAttacking;
    List<GameObject> targets;
    public float attackRadius;
    public bool friendlyFire = false;
    public float damage, reloadSpeed;

    public UnityEvent attackEvent;
    Animation attackAnimation;
    void Start()
    {
        gwp = GetComponent<GenericWeaponManager>();
        gwp.reloadTime = reloadSpeed;
        gwp.weaponDamage = damage;

        if (attackEvent == null) attackEvent = new UnityEvent();
        attackEvent.AddListener(Attack);

        gwp.attackEvent = attackEvent;
        targets = new List<GameObject>();

        attackAnimation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        targets.Clear();
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D item in hit)
        {
            if (item.CompareTag("Enemy") && friendlyFire)
                targets.Add(item.gameObject);
            else if (!item.CompareTag("Enemy") && !item.CompareTag("Level") && !item.CompareTag("Weapon"))
                targets.Add(item.gameObject);

        }
        gwp.targets = targets;

        if (attackAnimation != null) attackAnimation.Play();
    }

    
}
