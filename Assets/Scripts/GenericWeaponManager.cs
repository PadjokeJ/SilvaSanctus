using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericWeaponManager : MonoBehaviour
{
    public List<GameObject> targets;
    public float weaponDamage;
    public float reloadTime;
    public float weaponDistance;
    public float knockback;
    public ParticleSystem particle;

    public bool isAttacking;
    
    public UnityEvent attackEvent;

    public string weaponName;
    public string weaponDescription;
  
    public void Attack()
    {
        if (attackEvent != null)
            attackEvent.Invoke();
        else
            Debug.LogError("YOUR WEAPON " + this.gameObject.name + "HAS NO ATTACK EVENT");
    }
}
