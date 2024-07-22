using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnPoint : MonoBehaviour
{
    public Vector3 offset;

    private void Awake()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(TeleportPlayer(player));
    }

    private void OnDrawGizmos()
    {
        Vector3 pos;

        pos = offset + transform.position;

        Gizmos.color = Color.grey;

        Gizmos.DrawLine(pos + Vector3.right, pos + Vector3.left);
        Gizmos.DrawLine(pos + Vector3.up, pos + Vector3.down);

    }

    IEnumerator TeleportPlayer(GameObject player)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        player.transform.position = transform.position + offset;
    }
}
