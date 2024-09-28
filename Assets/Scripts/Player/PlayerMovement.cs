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

    [SerializeField]
    float dashForce;
    Vector2 currentForce = new Vector2();
    public float dashMaxTime, dashReload;
    float dashTime;

    bool toDash;
    //needs
    Rigidbody2D rb;
    //settings
    public bool toggleSprint;

    GameObject mainCam;

    Animator animator;
    SpriteRenderer spriteRenderer;

    public GameObject shadowPrefab;

    Health health;
    
    void Awake()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main.gameObject;

        spriteRenderer = GetComponent<SpriteRenderer>();

        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("XVel", moveV2.normalized.x);
        animator.SetFloat("YVel", moveV2.normalized.y);

        rb.velocity += moveV2.normalized * speed;
        normalizedMagnitude = new Vector2(Mathf.Abs(rb.velocity.normalized.x), Mathf.Abs(rb.velocity.normalized.y));
        clampedSpeed = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        rb.velocity = new Vector2(clampedSpeed.x * normalizedMagnitude.x, clampedSpeed.y * normalizedMagnitude.y);
        rb.velocity += currentForce;
        if(toDash)
        {
            currentForce = dashForce * moveV2;
            StartCoroutine(DashTrail());
            toDash = false;
            health.canTakeDamage = false;
        }
        dashTime += Time.deltaTime;
        if (dashTime > dashMaxTime)
        {
            currentForce = Vector2.zero;
            
        }
    }
    public void onMoveUpdate(InputAction.CallbackContext context)
    {
        moveV2 = context.ReadValue<Vector2>();
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && dashTime > dashMaxTime + dashReload)
        {
            toDash = true;
            dashTime = 0f;
        }

    }

    IEnumerator DashTrail()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate<GameObject>(shadowPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            yield return new WaitForSeconds(0.05f);
            
        }
        yield return new WaitForSeconds(0.25f);
        health.canTakeDamage = true;
    }
}
