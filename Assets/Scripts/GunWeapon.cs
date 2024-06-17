using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunWeapon : Weapon
{


    void Awake()
    {
        gWP = GetComponent<GenericWeaponManager>();
        gWP.attackEvent = attackEvent;
        gWP.reloadTime = reloadTime;
        gWP.weaponDamage = weaponDamage;
        gWP.weaponDistance = weaponDistance;
        gWP.knockback = knockback;
        gWP.particle = particle;

        weaponDistance = 1f;

        targets = new List<GameObject>();

        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
    }

    public void Attack()
    {
        targets.Clear();

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 1f, transform.right);

        if(hit.collider != null)
        {
            targets.Add(hit.collider.gameObject);
        }

        attackAnimation.Play();
        gWP.targets = targets;
    }
}
