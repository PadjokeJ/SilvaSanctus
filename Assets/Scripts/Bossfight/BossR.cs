using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossR : MonoBehaviour
{
    BossHealth health;

    bool isSpawned;

    string state = "None";

    GameObject player;

    public float speed;

    string[] states = { "Chase", "LongRange", "ShortRange" };
    private void Awake()
    {
        health = GetComponent<BossHealth>();
        player = Health.playerInstance.gameObject;

        isSpawned = true;
    }
    public void Spawn()
    {

    }

    public void TakeDamage()
    {
        health.health -= 1;
        health.ChangeHealthBar();
    }

    private void Update()
    {
        if(isSpawned)
        {
            if (state == "None")
            {
                if (Random.Range(0, 100) > 30)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) > 10f)
                    {

                    }else
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
            timeSpentChasing = (1 / 60f);

            playerDirection = player.transform.position - transform.position;
            playerDirection = playerDirection.normalized;

            transform.position += playerDirection * speed;
        }

        yield return new WaitForSeconds(2f);
        state = "None";
    }
}
