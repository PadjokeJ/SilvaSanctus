using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeProjectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    bool hasHitPlayer = false;

    public ParticleSystem trail;

    private void Awake()
    {
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Player") && !hasHitPlayer)
        {
            collision.GetComponent<Health>().takeDamage(2f);
            hasHitPlayer = true;
        }

        trail.Stop();

        var emissionModule = trail.emission;
        emissionModule.enabled = false;

        Destroy(transform.GetChild(0).gameObject);
        Destroy(this.gameObject, 1.5f);
    }
}
