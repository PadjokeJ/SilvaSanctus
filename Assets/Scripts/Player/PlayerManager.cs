using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    void Awake()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        //
    }

}
