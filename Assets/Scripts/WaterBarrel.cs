using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBarrel : MonoBehaviour
{
    public ParticleSystem explosion;

    public void Die()
    {
        explosion.Play();
        explosion.transform.SetParent(null);

        AttackBoss();

        Destroy(explosion.gameObject, 4);
        Destroy(this.gameObject);
    }

    void AttackBoss()
    {
        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, 5f, mask);

        foreach (Collider2D collider in collision)
        {
            if (collider.CompareTag("R"))
            {
                collider.GetComponent<BossR>().TakeDamage();
            }
        }
    }
}
