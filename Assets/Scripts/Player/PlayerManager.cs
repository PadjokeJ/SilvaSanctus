using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    Pause pauseCanvas;
    void Awake()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        pauseCanvas = FindAnyObjectByType<Pause>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        //
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
            pauseCanvas.TogglePause();
    }
}
