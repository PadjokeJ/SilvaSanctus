using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Hurt()
    {
        animator.SetTrigger("Hurt");
        Debug.Log("hurt triggered");
    }
}
