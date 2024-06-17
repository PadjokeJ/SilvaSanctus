using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public List<GameObject> targets;
    public float weaponDamage;
    public float reloadTime;
    public float weaponDistance;
    public float knockback;
    public ParticleSystem particle;

    public bool isAttacking;

    public UnityEvent attackEvent;

    public Animation attackAnimation;

    public GenericWeaponManager gWP;
/*  void Awake()
    {
        gWP = GetComponent<GenericWeaponManager>();
        gWP.attackEvent = attackEvent;
        gWP.reloadTime = reloadTime;
        gWP.weaponDamage = weaponDamage;
        gWP.weaponDistance = weaponDistance;
        gWP.knockback = knockback;
        gWP.particle = particle;


        targets = new List<GameObject>();
    }

    public void Attack()
    {
        targets.Clear();
        
        

        gWP.targets = targets;
    }
*/
}
