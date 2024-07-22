using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    BoxCollider2D doorCollider;
    bool hasPlayerVanquishedEnemies = false;
    private void Awake()
    {
        foreach (BoxCollider2D collider in transform.GetComponents<BoxCollider2D>())
        {
            if (!collider.isTrigger)
            {
                doorCollider = collider;
                break;
            }
        }
        doorCollider.enabled = false;
    }
    public void Open()
    {
        doorCollider.enabled = false;
        hasPlayerVanquishedEnemies = true;
    }
    public void Close()
    {
        if (!hasPlayerVanquishedEnemies)
            doorCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Close();
    }

    private void OnDrawGizmos()
    {
        List<BoxCollider2D> colliders = new List<BoxCollider2D>();

        colliders = new List<BoxCollider2D>(transform.GetComponents<BoxCollider2D>());

        foreach(BoxCollider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position + new Vector3(collider.offset.x, collider.offset.y), collider.size);
            }
            if (!collider.isTrigger)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + new Vector3(collider.offset.x, collider.offset.y), collider.size);
            }
        }
    }
}
