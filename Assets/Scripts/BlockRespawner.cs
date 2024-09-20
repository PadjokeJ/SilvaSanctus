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
        spawnedObject = Instantiate<GameObject>(objectPrefab);
        spawnedObject.transform.position = transform.position;

        while (spawnedObject != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(timeBetweenRespawns);

        StartCoroutine(SpawnObject());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
