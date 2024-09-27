using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeLag : MonoBehaviour
{
    List<Transform> children = new List<Transform>();
    GameObject player;

    bool areChildrenEnabled = true;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));

        }

        player = FindAnyObjectByType<PlayerAttack>().gameObject;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 20f && areChildrenEnabled)
        {
            DespawnObjects();
        }

        if (Vector3.Distance(player.transform.position, transform.position) < 20f && !areChildrenEnabled)
        {
            SpawnObjects();
        }

    }

    void DespawnObjects()
    {
        areChildrenEnabled = false;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
    }

    void SpawnObjects()
    {
        areChildrenEnabled = true;
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }
    }
}
