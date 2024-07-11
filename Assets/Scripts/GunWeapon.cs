using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunWeapon : Weapon
{
    LineRenderer lr;
    float fadeTime = 0.5f;
    float timeSinceShot = 0f;

    float hitDistance = 0f;

    public float offsetAngle = 0f, multiplierOvertime = 0f, maxAngle = 0f;
    float time;

    CameraManager cm;

    public float shakeAmplitude = 5f, shakeFrequency = 20f, shakeTime = 0.5f;


    
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

        lr = GetComponentInChildren<LineRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();

        cm = FindObjectOfType<CameraManager>();
    }

    void Update()
    {
        lr.SetPosition(0, Vector3.Lerp(lr.GetPosition(0), lr.GetPosition(1), 0.1f / hitDistance));
        
    }

    private void FixedUpdate()
    {
        time -= reloadTime * 4f;
        time = Mathf.Clamp(time, 0f, 20f);
    }

    public void Attack()
    {
        targets.Clear();
        hitDistance = 20f; // in case we dont get a hit, so we dont mess up the graphics

        time += 1f;
        
        float angle = Mathf.Clamp(offsetAngle * multiplierOvertime * Mathf.Max(0f, Mathf.Log10(time) + 1f) * Random.Range(-1f, 1f), 0f, maxAngle);
        angle += transform.rotation.eulerAngles.z; // in degrees
        
        angle = Mathf.Deg2Rad * angle;

        Vector3 shootDir;
        if (!float.IsInfinity(angle))
        {
            shootDir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            shootDir.Normalize();
        }
        else
            shootDir = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 0.5f, shootDir);

        if (hit.collider != null)
        {
            hitDistance = hit.distance;

            targets.Add(hit.collider.gameObject);
        }
        foreach(GameObject target in targets)
        {
            if(target.TryGetComponent<Health>(out Health healthScript))
                healthScript.takeDamage(weaponDamage);
        }

        // graphics:
        attackAnimation.Play();
        timeSinceShot = 0f;
        lr.SetPosition(0, transform.position + transform.right * 0.5f);
        lr.SetPosition(1, lr.GetPosition(0) + shootDir * hitDistance * 1.1f);
        particle.Play();

        cm.CameraShake(shakeAmplitude, shakeFrequency, shakeTime);

        gWP.targets = targets;
    }
}
