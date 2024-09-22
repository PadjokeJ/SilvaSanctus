using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject door;
    BossR bossR;
    void Awake()
    {
        door.SetActive(false);

        bossR = FindAnyObjectByType<BossR>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            door.SetActive(true);

            StartCoroutine(waitForPlayerToBeNearBoss());
        }
    }

    IEnumerator waitForPlayerToBeNearBoss()
    {
        GameObject player = Health.playerInstance.gameObject;
        Vector3 bossPosition = bossR.transform.position;

        while(Vector3.Distance(bossPosition, player.transform.position) > 10f)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bossR.Spawn();
    }
}
