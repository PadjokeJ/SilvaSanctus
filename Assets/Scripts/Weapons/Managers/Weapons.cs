using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon list")]

public class Weapons : ScriptableObject
{
    public GameObject[] weaponPrefabs;

    public int[] unlockedWeapons;
}
