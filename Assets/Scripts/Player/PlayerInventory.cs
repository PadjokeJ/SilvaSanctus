using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> weapons;
    public GameObject selectedWeapon;

    UiInventoryManager uiInventory;

    public int selectedSlot;

    // buffs?

    private void Awake()
    {
        uiInventory = FindAnyObjectByType<UiInventoryManager>();
        weapons.Add(WeaponTransfer.startingWeapon);
    }

    public void InstantiateWeapon(int index, GameObject parent)
    {
        if (selectedWeapon != null)
            Destroy(selectedWeapon);

        selectedSlot = index;

        selectedWeapon = Instantiate<GameObject>(weapons[index], parent.transform);
        uiInventory.SelectWeapon(index);
    }    

    public void AddWeapon(GameObject weapon)
    {
        weapons.Add(weapon);
        uiInventory.AddWeapon(weapon);
    }

    public void RemoveWeapon(int index)
    {
        uiInventory.RemoveWeapon(index);
        weapons.RemoveAt(index);
    }
}
