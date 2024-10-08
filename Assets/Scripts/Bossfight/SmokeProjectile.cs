using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeProjectile : MonoBehaviour
{
    public Vector3 direction;
    Vector3 dir2;
    public float speed;

    bool hasHitPlayer = false;

    public ParticleSystem trail;

    public bool isSine = false;

    float wave = 0;

    private void Awake()
    {
        Destroy(this.gameObject, 10f);
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        wave += Time.deltaTime * 180f * 2f;
        transform.position += direction * speed * Time.deltaTime;
        if (isSine)
        {
            dir2 = new Vector3(direction.y, direction.x);
            transform.position += dir2 * Mathf.Sin(Mathf.Deg2Rad * wave) * 12f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool toRemove = false;

        if(collision.CompareTag("Player") && !hasHitPlayer)
        {
            Health health = collision.GetComponent<Health>();
            health.takeDamage(2f);
            hasHitPlayer = true;
            toRemove = health.canTakeDamage;
        }
        if (!collision.CompareTag("R") && !collision.CompareTag("Player") && !collision.CompareTag("Barrel"))
        {
            toRemove = true;
        }

        if (toRemove)
        {
            trail.Stop();

            var emissionModule = trail.emission;
            emissionModule.enabled = false;

            Destroy(this.gameObject, 1.5f);

            speed = 0;
            isSine = false;

            hasHitPlayer = true;
        }
    }
}
