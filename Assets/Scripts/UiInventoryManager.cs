using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventoryManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    public GameObject weaponSlotPrefab;
    List<GameObject> weaponSlots;

    public float selectedHeight;
    public float normalHeight;
    public float spacing;
    void Awake()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        weaponSlots = new List<GameObject>();

        int iteration = 0;
        foreach(GameObject weapon in playerInventory.weapons)
        {
            weaponSlots.Add(Instantiate<GameObject>(weaponSlotPrefab, transform));
            weaponSlots[iteration].GetComponent<RectTransform>().anchoredPosition += new Vector2(iteration * spacing - 0.5f * spacing * playerInventory.weapons.Count, normalHeight);
            weaponSlots[iteration].transform.GetChild(0).GetComponent<Image>().sprite = playerInventory.weapons[iteration].GetComponentInChildren<SpriteRenderer>().sprite;
            iteration++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectWeapon(int index)
    {

    }
}
