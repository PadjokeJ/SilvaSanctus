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
    bool canAttack;
    bool isAttacking;
    public List<GameObject> targets;
    Vector2 mousePos;
    Vector2 playerPos, weapPos;
    public GenericWeaponManager gWP;
    SpriteRenderer sr;
    TrailRenderer tr;

    public float lerpTime;

    PlayerInventory playerInventory;

    bool weaponHasTrail = false;


    Animation attackAnimation;
    void Awake()
    {
        mainCam = Camera.main;

        playerInventory = GetComponent<PlayerInventory>();
        playerInventory.InstantiateWeapon(0);

        weapon = playerInventory.GetComponentInChildren<GenericWeaponManager>().gameObject;


        gWP = GetComponentInChildren<GenericWeaponManager>();

        

        attackAnimation = GetComponentInChildren<Animation>();
        weaponAnimator = attackAnimation.gameObject;

        sr = weapon.GetComponent<SpriteRenderer>();
        tr = weapon.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = transform.position;
        weaponDistance = gWP.weaponDistance;

        Vector3 weaponDir = CalculateWeaponDirection();

        if (!attackAnimation.isPlaying && !canAttack) weapon.transform.position = Vector3.Lerp(weapon.transform.position, playerPos + new Vector2(weaponDir.x, weaponDir.y) * weaponDistance, 0.05f);
        if (!attackAnimation.isPlaying) weapon.transform.position = Vector3.Lerp(weapon.transform.position, playerPos + new Vector2(weaponDir.x, weaponDir.y) * weaponDistance, lerpTime);
        Vector3 weapRot = weapon.transform.rotation.eulerAngles;


        if (!attackAnimation.isPlaying) weapon.transform.right = weapon.transform.position - transform.position;
        if(weapRot.z > 90 && weapRot.z < 270) weapon.transform.localScale = new Vector3(1, -1, 1);
        if(weapRot.z < 90 || weapRot.z > 270) weapon.transform.localScale = new Vector3(1, 1, 1);
        

        canAttack = reloadTime - deltaToReload < 0;
        deltaToReload += Time.deltaTime;
        if(canAttack && isAttacking)
        {
            Debug.Log(deltaToReload);
            weaponAnimator.transform.rotation = Quaternion.identity;
            weaponDir = CalculateWeaponDirection();
            weapon.transform.position = transform.position + new Vector3(weaponDir.x, weaponDir.y);

            attackAnimation.Play();

            targets = gWP.targets;
            deltaToReload = 0;
            if(targets != null)
            {
                foreach (GameObject item in targets)
                {
                    Debug.Log(item);

                    //ps.Play();
                    if (item.TryGetComponent<Health>(out Health health))
                    {
                        health.takeDamage(gWP.weaponDamage);
                    }
                    if (item.TryGetComponent<EnemyAI>(out EnemyAI eAI))
                    {
                        eAI.takeKB(transform, gWP.knockback);
                    }
                }
            }
        }

        if (weaponHasTrail)
        {
            if (attackAnimation.isPlaying)
            {
                //sr.enabled = false;
                tr.emitting = true;
            }
            else
            {
                //sr.enabled = true;
                tr.emitting = false;
            }
        }
             

    }
    public void Attack(InputAction.CallbackContext context)
    {
        isAttacking = context.performed;
        Debug.Log("Player prepares their sword");
    }
    public void GetWeapon(GameObject weap)
    {
        weapon = weap;
        reloadTime = weapon.GetComponent<WeaponManager>().reloadTime;
        damage = weapon.GetComponent<WeaponManager>().damage;
        weaponDistance = weapon.GetComponent<WeaponManager>().distFromPlayer;
        kb = weapon.GetComponent<WeaponManager>().knockBack;
        ps = weapon.GetComponent<ParticleSystem>();
    }
    public void GetMouseCoords(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    Vector2 CalculateWeaponDirection()
    {
        Vector3 worldMousePos = mainCam.ScreenToWorldPoint(mousePos);
        worldMousePos = new Vector3(worldMousePos.x, worldMousePos.y, 0f);
        Vector2 weaponDir = worldMousePos - transform.position;
        return weaponDir.normalized;
    }
}
