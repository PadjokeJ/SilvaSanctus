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
    float dist, timeSinceReached;
    public bool LOS, attacking = false;
    Vector2 deltaPos;
    Rigidbody2D rg;

    GenericWeaponManager gWP;
    float timeSinceLastAttack;
    float attackSpeed;
    UnityEvent attackEvent;

    Animation weaponAnimation;

    EnemyHealthBar ehb;

    List<Vector3> desirableDir = new List<Vector3>();

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gWP = GetComponentInChildren<GenericWeaponManager>();

        weaponAnimation = GetComponentInChildren<Animation>();

        ehb = FindObjectOfType<EnemyHealthBar>();
        Vector3 po = CalculateDirectionVector();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        LOS = !Physics2D.Linecast(transform.position, player.transform.position, 6);
        dist = Vector2.Distance(player.transform.position, transform.position);
        if (LOS && dist > minDist && dist < maxDist && !attacking) //move towards player
        {
            Vector2 dir = player.transform.position - transform.position;
            deltaPos = dir.normalized * speed * Time.deltaTime;
            rg.velocity += deltaPos;
        }
        if(LOS && dist < minDist + minRange && fleeIfTooClose) //flee
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
                timeSinceLastAttack = 0f;
                attacking = true;
            }
            
        }
        if(attacking)
        {
            timeSinceReached += Time.deltaTime;
            if(timeSinceReached >= reactionTime)
            {
                timeSinceReached = 0f;
                weaponAnimation.Play();
                timeSinceLastAttack = 0f;
                attackEvent.Invoke();
                Debug.Log("ENEMY ATTACKS!!!!");
                foreach (GameObject item in gWP.targets)
                {
                    Debug.Log(item);
                    item.GetComponent<Health>().takeDamage(gWP.weaponDamage);
                }

                attacking = false;
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
    Vector3 CalculateDirectionVector()
    {
        Vector3 dir = new Vector3();
        

        float angle = 0f;
        //launch 8 different linecasts to get where it can move
        for (int i = 0; i < 8; i++)
        {
            angle = i * 45f;
            desirableDir.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0));
            Debug.Log(desirableDir[i]);
        }

        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    { 
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        foreach (Vector3 dir in desirableDir)
            Gizmos.DrawLine(transform.position, transform.position + dir);
    }
}
