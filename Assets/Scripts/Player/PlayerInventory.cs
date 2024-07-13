using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> weapons;
    public GameObject selectedWeapon;

    UiInventoryManager uiInventory;

    // buffs?

    private void Awake()
    {
        uiInventory = FindAnyObjectByType<UiInventoryManager>();
    }

    public void InstantiateWeapon(int index, GameObject parent)
    {
        if (selectedWeapon != null)
            Destroy(selectedWeapon);

        selectedWeapon = Instantiate<GameObject>(weapons[index], parent.transform);
        uiInventory.SelectWeapon(index);
    }    

    public void AddWeapon(GameObject weapon)
    {
        weapons.Add(weapon);
        uiInventory.AddWeapon(weapon);
    }
}
