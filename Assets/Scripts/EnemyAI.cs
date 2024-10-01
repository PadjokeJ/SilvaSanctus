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
    float attackTime;

    Animation weaponAnimation;

    EnemyHealthBar ehb;
    GameObject weapon;

    List<Vector3> desirableDir = new List<Vector3>();

    public float off;

    GameObject healthBar;
    Health healthScript;

    public float experienceGiven;

    public GameObject warnObject;
    Animator warnAnimator;

    string state = "moving";

    public bool multipleAttacks;
    public int maxRepeatedAttacks;
    public float repeatedAccuracy;

    public ParticleSystem deathParticles;

    public ListOfDoors listOfDoors;

    Animator animator;

    bool dead = false;

    LayerMask wallsMask;
    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gWP = GetComponentInChildren<GenericWeaponManager>();
        weapon = gWP.gameObject;

        weaponAnimation = GetComponentInChildren<Animation>();

        healthScript = GetComponent<Health>();
        ehb = FindObjectOfType<EnemyHealthBar>();
        
        Vector3 po = CalculateDirectionVector();

        healthBar = ehb.newHealthBar(this);
        ehb.updateHealthBar(healthBar, transform.position, healthScript.health, healthScript.maxHealth);


        warnObject = Instantiate<GameObject>(warnObject, transform);
        warnAnimator = warnObject.GetComponent<Animator>();
        warnObject.transform.position = transform.position + new Vector3(0, 1);

        /*if (weaponAnimation.clip != null)
            attackTime = weaponAnimation.clip.length;
        else*/
            attackTime = gWP.reloadTime;

        deathParticles = GameObject.FindWithTag("DeathParticles").GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();
        animator.speed = 0;

        wallsMask = LayerMask.GetMask("Walls");
    }

    void Update()
    {
        dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist < maxDist * 2f)
        {
            warnAnimator.SetBool("Attacking", attacking);
            timeSinceLastAttack += Time.deltaTime;

            if (state == "moving")
            {
                animator.speed = 1;
                

                LOS = !Physics2D.Linecast(transform.position, player.transform.position, wallsMask);
                if (LOS && dist > minDist && dist < maxDist) //move towards player
                {
                    deltaPos = CalculateDirectionVector().normalized * speed * Time.deltaTime;
                    rg.velocity += deltaPos;
                }
                if (LOS && dist < minDist + minRange && fleeIfTooClose) //flee
                {
                    deltaPos = CalculateDirectionVector().normalized * speed * fleeSpeedMultiplier * Time.deltaTime;
                    rg.velocity -= deltaPos;
                }

                animator.SetFloat("SpeedX", deltaPos.normalized.x);
                animator.SetFloat("SpeedY", deltaPos.normalized.y);

                SetWeaponDirection(1f);

                if (dist <= attackDist)
                {
                    attackSpeed = gWP.reloadTime;
                    attackEvent = gWP.attackEvent;

                    if (timeSinceLastAttack > attackSpeed)
                    {
                        state = "attacking";
                    }
                }

                

            }
            if (state == "attacking")
            {
                animator.speed = 0;
                if (!attacking)
                {
                    if (!multipleAttacks)
                        StartCoroutine(Attack());
                    else
                        StartCoroutine(MultipleAttacks());
                }
            }

            

            ehb.updateHealthBar(healthBar, transform.position, healthScript.health, healthScript.maxHealth);
        }
    }

    public IEnumerator Attack()
    {
        attacking = true;
        WarnAnimation();
        yield return new WaitForSeconds(reactionTime);

        weaponAnimation.Play();

        attackEvent.Invoke();

        foreach (GameObject item in gWP.targets)
        {
            if (item.TryGetComponent<Health>(out Health damageTarget))
                damageTarget.takeDamage(gWP.weaponDamage);
        }

        yield return new WaitForSeconds(gWP.reloadTime);
        timeSinceLastAttack = 0f;
        timeSinceReached = 0f;

        state = "moving";
        attacking = false;
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator MultipleAttacks()
    {
        attacking = true;
        WarnAnimation();
        yield return new WaitForSeconds(reactionTime);

        for (int i = 0; i < maxRepeatedAttacks; i++)
        {
            weaponAnimation.Play();

            attackEvent.Invoke();
            foreach (GameObject item in gWP.targets)
            {
                if (item.TryGetComponent<Health>(out Health damageTarget))
                    damageTarget.takeDamage(gWP.weaponDamage);
            }
            yield return new WaitForSeconds(gWP.reloadTime);
            SetWeaponDirection(repeatedAccuracy);
        }
        state = "moving";
        timeSinceLastAttack = 0f;
        timeSinceReached = 0f;
        attacking = false;
        yield return new WaitForEndOfFrame();
    }
    void SetWeaponDirection(float lerp)
    {
        Vector3 weaponDir = player.transform.position - transform.position;
        weaponDir = weaponDir.normalized;

        weapon.transform.position = Vector3.Lerp(weapon.transform.position, transform.position + weaponDir, lerp);
        weapon.transform.right = Vector3.Lerp(weapon.transform.right, weaponDir, lerp);

        Vector3 weapRot = weapon.transform.rotation.eulerAngles;
        if (weapRot.z > 90 && weapRot.z < 270) weapon.transform.localScale = new Vector3(1, -1, 1);
        if (weapRot.z < 90 || weapRot.z > 270) weapon.transform.localScale = new Vector3(1, 1, 1);
    }
    void WarnAnimation()
    {
        warnAnimator.SetTrigger("Warn");
    }
    public void Die()
    {
        if (dead == false)
        {
            dead = true;
            PlayerLevelling.GainExperience(experienceGiven);

            deathParticles.transform.position = transform.position;
            deathParticles.Emit(10);

            if (listOfDoors != null)
                listOfDoors.OnEnemyDeath();

            Destroy(healthBar);
            Destroy(this.gameObject);
        }
    }
    public void takeKB(Vector3 damageOrigin, float KnockBackStrength)
    {
        Vector3 kbDir = damageOrigin - transform.position;
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

        LayerMask mask = ~LayerMask.GetMask("Ignore Raycast");

        foreach (Vector3 direction in tempDir)
        {
            Vector3 offsetDirection;
            float angleOffset = off * -1f;
            for (int i = 0; i < 3; i++)
            {
                offsetDirection = Quaternion.Euler(0, 0, angleOffset) * desirableDir[iteration];
                RaycastHit2D[] tempResults = Physics2D.LinecastAll(transform.position, transform.position + offsetDirection * 1.5f, mask);
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

    void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        foreach (Vector3 dir in desirableDir)
        {
            Gizmos.DrawLine(transform.position, transform.position + dir * 1.5f);
        }

        // Range
        if (dist < maxDist) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        if (dist < 2 * maxDist)
            Gizmos.DrawWireSphere(transform.position, maxDist);
    }
    private void OnDestroy()
    {
        if (healthBar != null)
            Destroy(healthBar);
    }
}
