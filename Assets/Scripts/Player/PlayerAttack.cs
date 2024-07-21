using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    GameObject weapon, weaponAnimator;
    Camera mainCam;
    public float damage;
    float weaponDistance, kb;
    ParticleSystem ps;
    float reloadTime;
    float deltaToReload;
    public bool canAttack;
    public bool isAttacking;
    public List<GameObject> targets;
    Vector2 mousePos;
    Vector2 playerPos, weapPos;
    public GenericWeaponManager gWP;
    SpriteRenderer sr;
    TrailRenderer tr;

    int weaponIndex = 0;

    public float lerpTime;

    PlayerInventory playerInventory;

    bool weaponHasTrail = false;


    Animation attackAnimation;


    GameObject hand;

    PlayerInput playerInput;

    Vector2 centerOfScreen;

    public GameObject crosshair;
    void Awake()
    {
        mainCam = Camera.main;

        playerInventory = GetComponent<PlayerInventory>();

        hand = new GameObject("Hand");
        hand.transform.SetParent(this.transform);

        StartCoroutine(ChangeWeapon(0));

        playerInput = GetComponent<PlayerInput>();

        centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        crosshair.transform.position = mainCam.ScreenToWorldPoint(mousePos);
        crosshair.transform.position = new Vector2(crosshair.transform.position.x, crosshair.transform.position.y);



        if (attackAnimation != null && !attackAnimation.isPlaying)
            deltaToReload += Time.deltaTime;
        canAttack = gWP.reloadTime - deltaToReload < 0;

        Vector3 weaponDir = CalculateWeaponDirection();
        playerPos = transform.position;

        gWP.isAttacking = attackAnimation.isPlaying;
        hand.transform.position = Vector3.Lerp(hand.transform.position, playerPos + new Vector2(weaponDir.x, weaponDir.y) * gWP.weaponDistance, 0.5f);
        hand.transform.right = weaponDir;
        
        Vector3 weapRot = hand.transform.rotation.eulerAngles;
        if (weapRot.z > 90 && weapRot.z < 270) hand.transform.localScale = new Vector3(1, -1, 1);
        if (weapRot.z < 90 || weapRot.z > 270) hand.transform.localScale = new Vector3(1, 1, 1);

        if(canAttack && isAttacking)
        {
            canAttack = false;
            deltaToReload = 0f;

            gWP.Attack();
            gWP.isAttacking = true;
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        isAttacking = context.performed;
    }
    public void GetMouseCoords(InputAction.CallbackContext context)
    {
        if (playerInput.currentControlScheme == "Keyboard")
            mousePos = context.ReadValue<Vector2>();
        else if (context.ReadValue<Vector2>().magnitude > 0.5f)
            mousePos = context.ReadValue<Vector2>().normalized * 200f + centerOfScreen;
    }
    public void WeaponSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weaponIndex += 1;
            if (weaponIndex == playerInventory.weapons.Count)
                weaponIndex = 0;
            StartCoroutine(ChangeWeapon(weaponIndex));
        }
    }
    public void DropWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInventory.RemoveWeapon(weaponIndex);
            if (weaponIndex >= playerInventory.weapons.Count)
                weaponIndex = 0;
            StartCoroutine(ChangeWeapon(weaponIndex));
        }
    }
    Vector2 CalculateWeaponDirection()
    {
        Vector3 worldMousePos = mainCam.ScreenToWorldPoint(mousePos);
        worldMousePos = new Vector3(worldMousePos.x, worldMousePos.y, 0f);
        Vector2 weaponDir = worldMousePos - transform.position;
        return weaponDir.normalized;
    }

    IEnumerator ChangeWeapon(int index)
    {
        playerInventory.InstantiateWeapon(index, hand);
        yield return new WaitForEndOfFrame();

        weapon = playerInventory.GetComponentInChildren<GenericWeaponManager>().gameObject;

        gWP = GetComponentInChildren<GenericWeaponManager>();

        attackAnimation = GetComponentInChildren<Animation>();
        weaponAnimator = attackAnimation.gameObject;

        sr = weapon.GetComponent<SpriteRenderer>();
        tr = weapon.GetComponent<TrailRenderer>();
    }
}
