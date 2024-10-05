using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;


public class BossR : MonoBehaviour
{
    BossHealth health;

    bool isSpawned;

    string state = "None";

    GameObject player;

    public float speed;

    string[] states = { "Chase", "LongRange", "ShortRange" };

    public GameObject smokePrefab;

    public GameObject spriteObject;
    public GameObject particleObject;

    int phase = 1;

    bool touchingPlayer;

    CameraManager cm;

    IEnumerator damageTaker;

    bool dead = false;

    bool down = false;

    float damageOverTime = 1f;
    float waitPercent = 1f;
    float expReward = 20f;

    List<Vector3> positionOfBarrels = new List<Vector3>();

    public List<GameObject> enemies;

    private void Awake()
    {
        health = GetComponent<BossHealth>();
        player = Health.playerInstance.gameObject;

        particleObject.SetActive(false);

        cm = FindObjectOfType<CameraManager>();

        if (WeaponManaging.hardMode)
        {
            speed *= 1.2f;
            damageOverTime *= 1.5f;
            waitPercent = 0.75f;
            expReward = 40f;
        }

        StartCoroutine(GetBarrels());
    }

    private void Update()
    {
        if (isSpawned && !dead && !down)
        {
            if (state == "None")
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    StartCoroutine(LongRange());
                }
                else
                {
                    StartCoroutine(ChasePlayer());
                }
            }

            
        }

        if (health.health <= 0 && !dead && isSpawned)
            Die();
    }
    public void Spawn()
    {
        StartCoroutine(SpawnSequence());
        StartCoroutine(Hint());
        StartCoroutine(DOT());
    }

    IEnumerator SpawnSequence()
    {
        StartCoroutine(SlowlyZoomOut());
        yield return new WaitForSeconds(2f);
        particleObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        
        spriteObject.SetActive(false);

        health.SpawnHealthCanvas();

        yield return new WaitForSeconds(1f);

        isSpawned = true;
    }

    IEnumerator SlowlyZoomOut()
    {
        PixelPerfectCamera ppc = FindAnyObjectByType<PixelPerfectCamera>();

        int startingRes = ppc.refResolutionX;
        int endingRes = 480;

        for (int i = 0; i <= 25; i++)
        {
            ppc.refResolutionX = Mathf.RoundToInt(Mathf.Lerp(startingRes, endingRes, i / 50f));
            yield return new WaitForSeconds(0.01f);
        }

        ppc.refResolutionX = endingRes;
    }

    IEnumerator PhaseUp()
    {
        state = "phasing";
        phase++;

        yield return new WaitForSeconds(1f);

        ParticleSystem rParticles = particleObject.GetComponent<ParticleSystem>();
        var emissionModule = rParticles.emission;

        bool toActivate = false;

        for (int i = 0; i < 8; i++)
        {
            toActivate = !toActivate;
            emissionModule.enabled = !toActivate;
            spriteObject.SetActive(toActivate);
            cm.CameraShake(10, 30, 0.5f);
            yield return new WaitForSeconds(0.2f);
        }

        spriteObject.SetActive(true);
        cm.CameraShake(40, 30, 2f);
        yield return new WaitForSeconds(1f);
        spriteObject.SetActive(false);

        if (phase == 3)
        {
            StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(18f * waitPercent);
        }

        if (phase == 4)
        {
            StartCoroutine(RadialAttack());
            yield return new WaitForSeconds(10f);
        }

        if (phase == 5)
        {
            StartCoroutine(TeleportAround());
            yield return new WaitForSeconds(7 * 2.5f);
        }

        state = "None";
    }

    IEnumerator RadialAttack()
    {
        transform.position = transform.parent.position;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i <= 12; i++)
        {
            GameObject smokeObject = Instantiate<GameObject>(smokePrefab);
            smokeObject.transform.position = transform.position;


            yield return new WaitForSeconds(0.5f * waitPercent);
            cm.CameraShake(4, 30, 0.5f);

            Vector3 direction;
            direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * 30 * i), Mathf.Sin(Mathf.Deg2Rad * 30 * i));

            smokeObject.GetComponent<SmokeProjectile>().direction = direction;
        }
    }

    IEnumerator TeleportAround()
    {
        for (int i = 0; i <= 7; i++)
        {
            transform.position = Vector3.Lerp(positionOfBarrels[Random.Range(0, positionOfBarrels.Count - 1)], player.transform.position, 0.5f);
            yield return new WaitForSeconds(1f);
            StartCoroutine(FireProjectile());
            yield return new WaitForSeconds(0.5f * waitPercent);
        }
    }

    IEnumerator SpawnEnemies()
    {
        int prevIndex = 0;
        int random = 0;
        for (int i = 0; i <= 10; i++)
        {
            while (prevIndex == random)
                random = Random.Range(0, positionOfBarrels.Count - 1);
            prevIndex = random;
            transform.position = positionOfBarrels[random];
            yield return new WaitForSeconds(0.8f * waitPercent);

            GameObject enemy = Instantiate<GameObject>(enemies[Random.Range(0, enemies.Count - 1)], transform.position, Quaternion.identity);
            enemy.GetComponent<Health>().health *= 2f;
            enemy.GetComponent<Health>().maxHealth *= 2f;
            enemy.GetComponent<EnemyAI>().maxDist *= 5f;
            

            yield return new WaitForSeconds(1f * waitPercent);
        }
    }



    IEnumerator DeathSequence()
    {
        dead = true;

        ParticleSystem rParticles = particleObject.GetComponent<ParticleSystem>();
        var emissionModule = rParticles.emission;
        

        for (int time = 0; time < 25; time++)
        {
            emissionModule.rateOverTime = 50f - time/2f;
            yield return new WaitForSeconds(0.1f);
            cm.CameraShake(20, 30, 2f);
        }

        emissionModule.rateOverTime = 0f;
        yield return new WaitForSeconds(2f);

        PlayerLevelling.GainExperience(expReward);

        EndManager endManager = FindAnyObjectByType<EndManager>();


        endManager.PlayerWins();
    }

    public void Die()
    {
        touchingPlayer = false;
        if (!dead)
        {
            StopAllCoroutines();
            StartCoroutine(DeathSequence());
        }
    }

    public void MakeDowned()
    {
        down = true;
        StopAllCoroutines();
        StartCoroutine(DOT());

        spriteObject.SetActive(true);
    }

    public void TakeDamage(float value)
    {
        health.health -= value;
        health.ChangeHealthBar();

    }

    void ToNextPhase()
    {
        StopAllCoroutines();
        StartCoroutine(PhaseUp());
        StartCoroutine(DOT());
        down = false;
    }

    public void TryTakeDamage(float damage)
    {
        if (down)
        {
            if (((5f - phase)) * 2f <= health.health)
                TakeDamage(damage / 10f);
            else if(health.health > 0.1f)
                ToNextPhase();
        }
    }

    

    IEnumerator Hint()
    {
        float startHealth = health.health;
        yield return new WaitForSeconds(30f);

        if (startHealth == health.health)
        {
            TutorialText.instance.PlayText("Try exploding a water barrel nearby to deal damage");
        }
    }

    IEnumerator ChasePlayer()
    {
        state = "chase";
        float timeSpentChasing = 0f;
        Vector3 playerDirection;

        float dir = 1;

        while (timeSpentChasing < 3f)
        {
            yield return new WaitForSeconds(1 / 60f);
            timeSpentChasing += (1 / 60f);

            playerDirection = player.transform.position - transform.position;
            playerDirection = playerDirection.normalized;

            transform.position += playerDirection * speed * phase * dir;
        }

        yield return new WaitForSeconds(1f * waitPercent);
        state = "None";
    }

    IEnumerator LongRange()
    {
        state = "Attacking";

        for (int i = 0; i < phase; i++)
        {
            StartCoroutine(FireProjectile());
            yield return new WaitForSeconds(1f * waitPercent);
        }

        yield return new WaitForSeconds(2f * waitPercent);
        state = "None";
    }

    IEnumerator FireProjectile()
    {
        GameObject smokeObject = Instantiate<GameObject>(smokePrefab);
        smokeObject.transform.position = transform.position;


        yield return new WaitForSeconds(0.5f * waitPercent);
        cm.CameraShake(4, 30, 0.5f);


        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection = playerDirection.normalized;


        smokeObject.GetComponent<SmokeProjectile>().direction = playerDirection;
        if (Random.Range(0f, 1f) < 0.1f)
            smokeObject.GetComponent<SmokeProjectile>().isSine = true;
    }

    IEnumerator DOT()
    {
        Health playerHealth = Health.playerInstance;

        while (true)
        {
            if (touchingPlayer && !dead)
                playerHealth.takeDamage(damageOverTime);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            touchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchingPlayer = false;
        }

    }

    IEnumerator GetBarrels()
    {
        yield return new WaitForSeconds(5f);
        var foundBarrelObjects = FindObjectsOfType<BlockRespawner>();
        foreach (BlockRespawner barrel in foundBarrelObjects)
        {
            positionOfBarrels.Add(barrel.transform.position);
        }
    }
}
