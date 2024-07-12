using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventoryManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    public GameObject weaponSlotPrefab;
    List<GameObject> weaponSlots;
    List<RectTransform> transforms = new List<RectTransform>();
    int selectedSlot;

    public float selectedHeight;
    public float normalHeight;
    public float spacing;

    float offset;
    void Awake()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        weaponSlots = new List<GameObject>();

        offset = 0.5f * spacing * playerInventory.weapons.Count;

        int iteration = 0;
        foreach(GameObject weapon in playerInventory.weapons)
        {
            weaponSlots.Add(Instantiate<GameObject>(weaponSlotPrefab, transform));
            transforms.Add(weaponSlots[iteration].GetComponent<RectTransform>());
            transforms[iteration].anchoredPosition = new Vector2(iteration * spacing - offset, normalHeight);
            weaponSlots[iteration].transform.GetChild(0).GetComponent<Image>().sprite = playerInventory.weapons[iteration].GetComponentInChildren<SpriteRenderer>().sprite;
            iteration++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int iteration = 0;
        foreach(RectTransform slot in transforms)
        {
            if (iteration != selectedSlot)
                slot.anchoredPosition = Vector2.Lerp(slot.anchoredPosition, new Vector2(iteration * spacing - offset, normalHeight), 0.5f);
            else
                slot.anchoredPosition = Vector2.Lerp(slot.anchoredPosition, new Vector2(iteration * spacing - offset, selectedHeight), 0.5f);
            iteration++;
        }
    }

    public void SelectWeapon(int index)
    {
        selectedSlot = index;
    }
}
