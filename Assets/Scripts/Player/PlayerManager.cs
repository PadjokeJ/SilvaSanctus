using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    Pause pauseCanvas;

    EndManager endManager;

    void Awake()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        pauseCanvas = FindAnyObjectByType<Pause>();

        endManager = FindAnyObjectByType<EndManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        endManager.PlayerLoses();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
            pauseCanvas.TogglePause();
    }
}
