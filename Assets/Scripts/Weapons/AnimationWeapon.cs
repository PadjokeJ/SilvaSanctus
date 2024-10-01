using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWeapon : Weapon
{
    public float attackRadius;
    CameraManager cm;
    public float shakeAmplitude, shakeFrequency, shakeTime;

    public ParticleSystem dustParticles;

    public GameObject pointOfContact;

    bool hitOpponent = false;

    GameObject projPrefab;
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
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(pointOfContact.transform.position, attackRadius);
        foreach (Collider2D item in hit)
        {
            if (item.gameObject.CompareTag("Enemy")) hitOpponent = true;
            if(item.gameObject.CompareTag("Enemy") || item.gameObject.CompareTag("Chest") || item.gameObject.CompareTag("Barrel"))
                targets.Add(item.gameObject);
            if(item.gameObject.TryGetComponent<BossR>(out BossR bossR))
            {
                bossR.TryTakeDamage(weaponDamage);
            }
        }
        foreach(GameObject target in targets)
        {
            if(target.TryGetComponent<Health>(out Health healthScript))
            {
                healthScript.takeDamage(weaponDamage);
            }    
        }
    }
    public void Shake()
    {
        cm.CameraShake(shakeAmplitude, shakeFrequency, shakeTime);
    }
    public void SmashParticles()
    {
        dustParticles.Play();
    }
    public void hitParticles()
    {
        if(hitOpponent)
        {
            dustParticles.Play();
        }
    }
    public void ResetWeapon()
    {
        hitOpponent = false;
    }
    
    public void InstantiateProjectile()
    {
        GameObject gO = new GameObject();
        gO.transform.position = pointOfContact.transform.position;
        gO.name = "Projectile";

        Projectile projectile = gO.AddComponent<Projectile>();
        float rotation = transform.rotation.eulerAngles.z;

        projectile.direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * rotation), Mathf.Sin(Mathf.Deg2Rad * rotation));
        projectile.speed = 5f;
        projectile.damage = weaponDamage;

        Destroy(gO, 1f);
    }

    public void EnableEmission()
    {
        var emissionModule = dustParticles.emission;
        emissionModule.enabled = true;
        dustParticles.Play();
    }

    public void DisableEmission()
    {
        var emissionModule = dustParticles.emission;
        emissionModule.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Gizmos.DrawWireSphere(pointOfContact.transform.position, attackRadius);
    }
}
