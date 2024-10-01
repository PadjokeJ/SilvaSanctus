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
    private void Awake()
    {
        health = GetComponent<BossHealth>();
        player = Health.playerInstance.gameObject;

        particleObject.SetActive(false);

        cm = FindObjectOfType<CameraManager>();
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
    }
    public void Spawn()
    {
        StartCoroutine(SpawnSequence());
        StartCoroutine(Hint());
        StartCoroutine(DOT());
    }

    IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(2f);
        particleObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        
        spriteObject.SetActive(false);

        health.SpawnHealthCanvas();

        yield return new WaitForSeconds(1f);

        isSpawned = true;

        FindAnyObjectByType<PixelPerfectCamera>().refResolutionX = 480;

    }

    IEnumerator PhaseUp()
    {
        state = "phasing";

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
        state = "None";
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

        PlayerLevelling.GainExperience(20f);

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
        phase++;
        StartCoroutine(PhaseUp());
        StartCoroutine(DOT());
        down = false;
    }

    public void TryTakeDamage(float damage)
    {
        if (down)
        {
            if (((5f - phase)) * 2f < health.health)
                TakeDamage(damage / 10f);
            else
                ToNextPhase();
        }
    }

    

    IEnumerator Hint()
    {
        float startHealth = health.health;
        yield return new WaitForSeconds(30f);

        if (startHealth == health.health)
        {
            TutorialText.instance.PlayText("R hates water explosions. Get him close to one and explode it to damage him");
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

        yield return new WaitForSeconds(1f);
        state = "None";
    }

    IEnumerator LongRange()
    {
        state = "Attacking";

        for (int i = 0; i < phase; i++)
        {
            GameObject smokeObject = Instantiate<GameObject>(smokePrefab);
            smokeObject.transform.position = transform.position;


            yield return new WaitForSeconds(0.5f);
            cm.CameraShake(4, 30, 0.5f);


            Vector3 playerDirection = player.transform.position - transform.position;
            playerDirection = playerDirection.normalized;


            smokeObject.GetComponent<SmokeProjectile>().direction = playerDirection;
            if (Random.Range(0f, 1f) < 0.25f * phase)
                smokeObject.GetComponent<SmokeProjectile>().isSine = true;
        }

        yield return new WaitForSeconds(2f);
        state = "None";
    }

    IEnumerator DOT()
    {
        Health playerHealth = Health.playerInstance;

        while (true)
        {
            if (touchingPlayer && !dead)
                playerHealth.takeDamage(1f);
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
}
