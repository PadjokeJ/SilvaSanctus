using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    GameObject player;
    public bool losesInterestIfNoLOS, fleeIfTooClose;
    public float speed;
    public float maxDist, minDist, minRange, fleeSpeedMultiplier, attackDist, reactionTime;
    float dist;
    bool LOS;
    Vector2 deltaPos;
    Rigidbody2D rg;

    GenericWeaponManager gWP;
    float timeSinceLastAttack;
    float attackSpeed;
    UnityEvent attackEvent;

    Animation weaponAnimation;

    EnemyHealthBar ehb;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gWP = GetComponentInChildren<GenericWeaponManager>();

        weaponAnimation = GetComponentInChildren<Animation>();

        ehb = FindObjectOfType<EnemyHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        LOS = !Physics2D.Linecast(transform.position, player.transform.position, 6);
        dist = Vector2.Distance(player.transform.position, transform.position);
        if (LOS && dist > minDist && dist < maxDist)
        {
            Vector2 dir = player.transform.position - transform.position;
            deltaPos = dir.normalized * speed * Time.deltaTime;
            rg.velocity += deltaPos;
        }
        if(LOS && dist < minDist + minRange && fleeIfTooClose)
        {
            Vector2 dir = player.transform.position - transform.position;
            deltaPos = dir.normalized * speed * fleeSpeedMultiplier * Time.deltaTime;
            rg.velocity -= deltaPos;
        }
        if(dist <= attackDist)
        {
            attackSpeed = gWP.reloadTime;
            attackEvent = gWP.attackEvent;

            if (attackSpeed < timeSinceLastAttack)
            {
                weaponAnimation.Play();
                timeSinceLastAttack = 0f;
                attackEvent.Invoke();
                Debug.Log("ENEMY ATTACKS!!!!");
                foreach (GameObject item in gWP.targets)
                {
                    Debug.Log(item);
                    item.GetComponent<Health>().takeDamage(gWP.weaponDamage);
                }
            }
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
        ehb.SpawnHealthBars();
        
        
    }
    public void takeKB(Transform playerTransform, float KnockBackStrength)
    {
        Vector3 kbDir = playerTransform.position - transform.position;
        Vector2 kbForce = kbDir.normalized * -KnockBackStrength;
        rg.AddForce(kbForce, ForceMode2D.Impulse);
    }
}
