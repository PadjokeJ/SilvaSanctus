using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject weapon;
    Camera mainCam;
    float damage;
    float weaponDistance, kb;
    ParticleSystem ps;
    int reloadTime;
    int deltaToReload;
    bool canAttack;
    bool isAttacking;
    public List<GameObject> targets;
    Vector2 mousePos;
    Vector2 playerPos, weapPos;
    GenericWeaponManager gWP;
    void Awake()
    {
        mainCam = Camera.main;
        gWP = GetComponentInChildren<GenericWeaponManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = transform.position;
        Vector2 playerScreenPos = mainCam.WorldToScreenPoint(playerPos);
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2) + mousePos - playerScreenPos);
        weapon.transform.position = playerPos + mouseWorldPos.normalized * weaponDistance;
        Vector3 mousePosV3 = mainCam.ScreenToWorldPoint(mousePos);
        Vector3 weapRot = weapon.transform.rotation.eulerAngles;
        mousePosV3.z = 0;
        weapon.transform.right = mousePosV3 - weapon.transform.position;
        if(weapRot.z > 90 && weapRot.z < 270) weapon.transform.localScale = new Vector3(1, -1, 1);
        if(weapRot.z < 90 || weapRot.z > 270) weapon.transform.localScale = new Vector3(1, 1, 1);

        canAttack = reloadTime - deltaToReload < 0;
        deltaToReload++;
        if(canAttack && isAttacking)
        {
            targets = gWP.targets;
            deltaToReload = 0;
            if(targets != null)
            {
                foreach (GameObject item in targets)
                {
                    Debug.Log(item);
                    ps.Play();
                    if (item.TryGetComponent<Health>(out Health health))
                    {
                        health.takeDamage(damage);
                    }
                    if (item.TryGetComponent<EnemyAI>(out EnemyAI eAI))
                    {
                        eAI.takeKB(transform, kb);
                    }
                }
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
}
