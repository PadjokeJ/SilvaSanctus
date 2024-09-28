using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRock : MonoBehaviour
{
    public Sprite[] sprites; 

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length - 1)];
    }
}
