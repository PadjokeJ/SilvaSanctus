using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericWeaponManager : MonoBehaviour
{
    public List<GameObject> targets;
    public float weaponDamage;
    public float reloadTime;

    public bool isAttacking;
    
    public UnityEvent attackEvent;

    public string weaponName;
    public string weaponDescription;
  
}
