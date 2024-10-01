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

    string weaponOwner;

    public AudioClip gunshotAudio;

    Vector3 shootPos;
    
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

        weaponOwner = transform.parent.parent.tag;

        shootPos = lr.GetPosition(1);
    }

    IEnumerator LerpShotTracer(Vector3 startPos, Vector3 endPos, float hitDistance)
    {
        Color start = Color.white;
        Color end = start;
        end.a = 0f;

        for (int i = 0; i < 20; i++)
        {
            lr.endColor = Color.Lerp(start, end, i / 20f);
            lr.startColor = Color.Lerp(start, end, i / 20f);
            yield return new WaitForSeconds(0.01f);
        }
        lr.endColor = end;
        lr.startColor = end;
    }

    private void FixedUpdate()
    {
        time -= reloadTime * 4f;
        time = Mathf.Clamp(time, 0f, 20f);
    }

    public void Attack()
    {
        if(gunshotAudio != null)
            AudioManager.instance.PlayAudio(gunshotAudio, transform.position, 1f, 0.1f);
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

        LayerMask mask = ~LayerMask.GetMask("Ignore Raycast");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 0.6f, shootDir, 80, mask);

        if (hit.collider != null)
        {
            hitDistance = hit.distance;
            if(!hit.collider.CompareTag(weaponOwner))
                targets.Add(hit.collider.gameObject);
        }
        foreach(GameObject target in targets)
        {
            if(target.TryGetComponent<Health>(out Health healthScript))
                healthScript.takeDamage(weaponDamage);
            else if (target.TryGetComponent<BossR>(out BossR R))
            {
                R.TryTakeDamage(weaponDamage);
            }
        }

        // graphics:
        attackAnimation.Play();
        timeSinceShot = 0f;
        lr.SetPosition(0, transform.position);
        StopAllCoroutines();
        StartCoroutine(LerpShotTracer(transform.position, transform.position + shootDir * hitDistance + transform.right * 0.5f, hitDistance));
        lr.SetPosition(1, lr.GetPosition(0) + shootDir * hitDistance + transform.right * 0.5f);
        particle.Play();

        cm.CameraShake(shakeAmplitude, shakeFrequency, shakeTime);

        gWP.targets = targets;
    }
}
