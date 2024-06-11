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
            Vector2 dir = CalculateDirectionVector();
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
                    if(item.TryGetComponent<Health>(out Health damageTarget))
                        damageTarget.takeDamage(gWP.weaponDamage);
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

        desirableDir.Clear();
        float angle = 0f;
        //launch 8 different linecasts to get where it can move
        for (int i = 0; i < 32; i++)
        {
            angle = i * 11.25f;
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
            RaycastHit2D[] tempResults = Physics2D.LinecastAll(transform.position, transform.position + direction * 2f);
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            foreach (RaycastHit2D result in tempResults)
            {
                if (!result.collider.gameObject.CompareTag("Enemy") && !result.collider.gameObject.CompareTag("Weapon") && !result.collider.gameObject.CompareTag("Player")) results.Add(result);
            }
            if (results.Count > 0) desirableDir[iteration] = Vector3.zero;
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
            Gizmos.DrawLine(transform.position, transform.position + dir * 2f);
            Debug.Log(dir);
        }
    }
}
