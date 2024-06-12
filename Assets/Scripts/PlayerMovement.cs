using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //temps
    [SerializeField] float speed, sprintSpeed;
    [SerializeField] float maxSpeed;
    Vector2 moveV2, normalizedMagnitude, clampedSpeed;
    bool sprinting;
    //needs
    Rigidbody2D rb;
    //settings
    public bool toggleSprint;

    GameObject mainCam;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += moveV2.normalized * speed;
        normalizedMagnitude = new Vector2(Mathf.Abs(rb.velocity.normalized.x), Mathf.Abs(rb.velocity.normalized.y));
        clampedSpeed = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        rb.velocity = new Vector2(clampedSpeed.x * normalizedMagnitude.x, clampedSpeed.y * normalizedMagnitude.y);
        if(sprinting)
        {
            rb.velocity += moveV2 * sprintSpeed;
        }
        /*if (rb.velocity.normalized.x > 0.1f) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        if (rb.velocity.normalized.x < -0.1f) transform.rotation = Quaternion.Euler(0f, 180f, 0f);*/

        mainCam.transform.position = transform.position + new Vector3(0, 0, -10f);
    }
    public void onMoveUpdate(InputAction.CallbackContext context)
    {
        moveV2 = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if(toggleSprint)
        {
            if(context.performed)
            {
                sprinting = !sprinting;
            }
        }else
        {
            sprinting = context.performed;
        }
        
    }
}
