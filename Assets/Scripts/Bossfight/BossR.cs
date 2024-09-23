using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

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

    IEnumerator damageTaker;
    private void Awake()
    {
        health = GetComponent<BossHealth>();
        player = Health.playerInstance.gameObject;

        particleObject.SetActive(false);
    }
    public void Spawn()
    {
        StartCoroutine(SpawnSequence());
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

    }

    public void Die()
    {
        touchingPlayer = false;
    }

    public void TakeDamage()
    {
        health.health -= 1;
        if (health.health <= 5)
            phase = 2;
        health.ChangeHealthBar();
    }

    private void Update()
    {
        if(isSpawned)
        {
            if (state == "None")
            {
                if (Random.Range(0, 100) > 50)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) > 4f)
                    {
                        StartCoroutine(LongRange());

                    }
                    else
                    {

                    }
                }
                else
                {
                    StartCoroutine(ChasePlayer());
                }

            }
        }
    }

    IEnumerator ChasePlayer()
    {
        state = "chase";
        float timeSpentChasing = 0f;
        Vector3 playerDirection;

        while (timeSpentChasing < 3f)
        {
            yield return new WaitForSeconds(1 / 60f);
            timeSpentChasing += (1 / 60f);

            playerDirection = player.transform.position - transform.position;
            playerDirection = playerDirection.normalized;

            transform.position += playerDirection * speed;
        }

        yield return new WaitForSeconds(2f);
        state = "None";
    }

    IEnumerator LongRange()
    {
        state = "Attacking";

        for (int i = 0; i < (phase - 1) * 2 + 1; i++)
        {
            GameObject smokeObject = Instantiate<GameObject>(smokePrefab);
            smokeObject.transform.position = transform.position;


            yield return new WaitForSeconds(0.5f);

            Vector3 playerDirection = player.transform.position - transform.position;
            playerDirection = playerDirection.normalized;


            smokeObject.GetComponent<SmokeProjectile>().direction = playerDirection;
        }

        yield return new WaitForSeconds(2f);
        state = "None";
    }

    IEnumerator DOT(Health playerHealth)
    {
        while (touchingPlayer)
        {
            playerHealth.takeDamage(1f);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            touchingPlayer = true;
            if(damageTaker == null)
            {
                damageTaker = DOT(collision.gameObject.GetComponent<Health>());
                StartCoroutine(damageTaker);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchingPlayer = false;
            if (damageTaker != null)
            {
                StopCoroutine(damageTaker);
            }
        }
    }
}
