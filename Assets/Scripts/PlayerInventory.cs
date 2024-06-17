using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> weapons;
    public GameObject selectedWeapon;

    // buffs?

    
    
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateWeapon(int index)
    {
        selectedWeapon = Instantiate<GameObject>(weapons[index], this.transform);
        selectedWeapon.transform.position = Vector3.zero;
        selectedWeapon.transform.rotation = Quaternion.identity;
    }
}
