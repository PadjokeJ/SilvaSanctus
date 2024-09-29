using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;

    public float speed;
    public float damage;

    private void Awake()
    {
        CircleCollider2D circleCollider2D = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = 0.4f;
        circleCollider2D.isTrigger = true;

        this.gameObject.layer = 2;

        damage = 0.5f;
    }

    private void Update()
    {
        transform.position += Time.deltaTime * speed * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health) && !collision.CompareTag("Player"))
        {
            health.takeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 0.4f);
    }
}
