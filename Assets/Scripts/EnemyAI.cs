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
    GameObject weapon;

    List<Vector3> desirableDir = new List<Vector3>();

    public float off;

    GameObject healthBar;

    Health healthScript;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gWP = GetComponentInChildren<GenericWeaponManager>();
        weapon = gWP.gameObject;

        weaponAnimation = GetComponentInChildren<Animation>();

        ehb = FindObjectOfType<EnemyHealthBar>();
        Vector3 po = CalculateDirectionVector();

        healthBar = ehb.newHealthBar(this);

        healthScript = GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        //weapon.transform.position = Vector3.Lerp(weapon.transform.position, transform.position + weaponDir, 1f);

        LOS = !Physics2D.Linecast(transform.position, player.transform.position, 6);
        dist = Vector2.Distance(player.transform.position, transform.position);
        if (LOS && dist > minDist && dist < maxDist && !attacking) //move towards player
        {
            deltaPos = CalculateDirectionVector().normalized * speed * Time.deltaTime;
            rg.velocity += deltaPos;
        }
        if(LOS && dist < minDist + minRange && fleeIfTooClose) //flee
        {
            deltaPos =  -CalculateDirectionVector().normalized * speed * fleeSpeedMultiplier * Time.deltaTime;
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

                weapon.transform.parent.rotation = Quaternion.identity;
                Vector3 weaponDir = player.transform.position - transform.position;
                weaponDir = weaponDir.normalized;
                weapon.transform.position = transform.position + weaponDir.normalized * 1.2f;


                attackEvent.Invoke();
                Debug.Log("ENEMY ATTACKS!!!!");
                foreach (GameObject item in gWP.targets)
                {
                    Debug.Log(item);
                    if(item.TryGetComponent<Health>(out Health damageTarget))
                        damageTarget.takeDamage(gWP.weaponDamage);
                }

                attacking = false;
            }
        }
        Debug.Log(healthScript.health);
        ehb.updateHealthBar(healthBar, transform.position, healthScript.health, healthScript.maxHealth);
    }
    public void Die()
    {
        Destroy(healthBar);
        Destroy(this.gameObject);
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

        desirableDir.Clear();
        float angle = 0f;
        //launch 8 different linecasts to get where it can move
        for (int i = 0; i < 16; i++)
        {
            angle = i * 22.5f;
            desirableDir.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0));
            
        }

        int iteration = 0;
        List<Vector3> tempDir = new List<Vector3>(desirableDir);

        //get dir to player
        dir = player.transform.position - transform.position;

        int j = 0;
        float dotProd = 0;
        foreach (Vector3 direction in tempDir)
        {
            dotProd = (Vector3.Dot(direction.normalized, dir.normalized) + 1f);
            desirableDir[j] = desirableDir[j] * dotProd * 0.5f;
            j++;
        }


        foreach (Vector3 direction in tempDir)
        {
            Vector3 offsetDirection;
            float angleOffset = off * -1f;
            for (int i = 0; i < 3; i++)
            {
                offsetDirection = Quaternion.Euler(0, 0, angleOffset) * desirableDir[iteration];
                RaycastHit2D[] tempResults = Physics2D.LinecastAll(transform.position, transform.position + offsetDirection * 1.5f);
                List<RaycastHit2D> results = new List<RaycastHit2D>();
                foreach (RaycastHit2D result in tempResults)
                {
                    if (result.collider.gameObject != this.gameObject && !result.collider.gameObject.CompareTag("Weapon") && !result.collider.gameObject.CompareTag("Player")) results.Add(result);
                }
                if (results.Count > 0) desirableDir[iteration] = Vector3.zero;
                angleOffset += off;
            }
            iteration++;
        }

        float maxDesirability = 0f, previousMax = 0f;

        foreach(Vector3 direction in desirableDir)
        {
            maxDesirability = Mathf.Max(maxDesirability, direction.magnitude);
            if (previousMax < maxDesirability) dir = direction;

            previousMax = maxDesirability;
        }

        return dir.normalized;
    }

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Debug.Log("Drawing gizmos");
        Gizmos.color = Color.blue;
        foreach (Vector3 dir in desirableDir)
        {
            Gizmos.DrawLine(transform.position, transform.position + dir * 1.5f);
            Debug.Log(dir);
        }
    }
}
