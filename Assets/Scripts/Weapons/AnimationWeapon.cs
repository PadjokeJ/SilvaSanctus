using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWeapon : Weapon
{
    public float attackRadius;
    CameraManager cm;
    public float shakeAmplitude, shakeFrequency, shakeTime;
    void Start()
    {
        gWP = GetComponent<GenericWeaponManager>();
        gWP.attackEvent = attackEvent;
        gWP.reloadTime = reloadTime;
        gWP.weaponDamage = weaponDamage;
        gWP.weaponDistance = weaponDistance;
        gWP.knockback = knockback;
        gWP.particle = particle;

        weaponDistance = 1f;

        //attackAnimation = GetComponent<Animation>();

        cm = FindObjectOfType<CameraManager>();
    }
    public void Attack()
    {
        attackAnimation.Rewind();
        attackAnimation.Play();
    }
    public void SendAttackCircle()
    {
        targets.Clear();
        cm.CameraShake(shakeAmplitude, shakeFrequency, shakeTime);
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D item in hit)
        {
            if(item.gameObject.CompareTag("Enemy") || item.gameObject.CompareTag("Chest"))
                targets.Add(item.gameObject);
        }
        foreach(GameObject target in targets)
        {
            if(target.TryGetComponent<Health>(out Health healthScript))
            {
                healthScript.takeDamage(weaponDamage);
            }    
        }
    }
}
