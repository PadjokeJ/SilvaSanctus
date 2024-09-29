using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    BoxCollider2D doorCollider;
    bool hasPlayerVanquishedEnemies = false;

    SpriteRenderer sr;

    [SerializeField] Sprite horDoor, verDoor; 

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

        sr = this.gameObject.AddComponent<SpriteRenderer>();

        if (doorCollider.size.x > 1.5f)
            sr.sprite = horDoor;
        if (doorCollider.size.y > 1.5f)
            sr.sprite = verDoor;
        sr.sortingOrder = -1;
        sr.enabled = false;
    }
    public void Open()
    {
        doorCollider.enabled = false;
        hasPlayerVanquishedEnemies = true;
        sr.enabled = false;
    }
    public void Close()
    {
        if (!hasPlayerVanquishedEnemies)
        {
            doorCollider.enabled = true;
            sr.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Close();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

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
