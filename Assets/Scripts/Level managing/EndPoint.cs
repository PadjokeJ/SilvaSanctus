using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public string endType;

    EndManager endManager;

    void Awake()
    {
        endManager = FindAnyObjectByType<EndManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (endType == "game")
            {
                endManager.PlayerWins();
            }
            if (endType == "level")
            {
                // finish the level
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
