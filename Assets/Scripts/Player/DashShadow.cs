using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashShadow : MonoBehaviour
{
    SpriteRenderer sr;

    float i = 0;

    Color start, end;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        start = Color.black;
        end = start;
        end.a = 0;
    }
    void FixedUpdate()
    {
        i += Time.fixedDeltaTime * 2f;

        sr.color = Color.Lerp(start, end, i);

        if (i > 1)
            Destroy(this.gameObject);
    }
}
