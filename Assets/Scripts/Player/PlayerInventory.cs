using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> weapons;
    public GameObject selectedWeapon;

    // buffs?

    public void InstantiateWeapon(int index)
    {
        if (selectedWeapon != null)
            Destroy(selectedWeapon);

        selectedWeapon = Instantiate<GameObject>(weapons[index], this.transform);
    }    
}
