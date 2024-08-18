using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManaging : MonoBehaviour
{
    public Weapons weapons;

    private void Awake()
    {
        WeaponTransfer.weaponsList = weapons;

        WeaponTransfer.startingWeapon = weapons.weaponPrefabs[0];
    }
}
