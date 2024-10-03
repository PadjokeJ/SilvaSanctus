using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRespawner : MonoBehaviour
{
    public float timeBetweenRespawns;

    public GameObject objectPrefab;
    GameObject spawnedObject;
    void Awake()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        spawnedObject = Instantiate<GameObject>(objectPrefab);
        spawnedObject.transform.position = transform.position;

        spawnedObject.GetComponent<WaterBarrel>().blockRespawner = this;

        while (spawnedObject != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(timeBetweenRespawns);

        StartCoroutine(SpawnObject());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
