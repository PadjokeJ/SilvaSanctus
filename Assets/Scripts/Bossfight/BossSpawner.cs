using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject door;
    BossR bossR;

    [SerializeField] Sprite sprite;
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
            SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            StartCoroutine(waitForPlayerToBeNearBoss());
        }
    }

    IEnumerator waitForPlayerToBeNearBoss()
    {
        GameObject player = Health.playerInstance.gameObject;
        Vector3 bossPosition = bossR.transform.position;

        while(Vector3.Distance(bossPosition, player.transform.position) > 6f)
        {
            yield return new WaitForSeconds(0.10f);
        }

        yield return new WaitForSeconds(0.5f);

        bossR.Spawn();
        
    }
}
