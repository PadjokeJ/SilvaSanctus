using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponObject;
    public void SetPlayerWeapon()
    {
        WeaponTransfer.startingWeapon = weaponObject;
    }
}
