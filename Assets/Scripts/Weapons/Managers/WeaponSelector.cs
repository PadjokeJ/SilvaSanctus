using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponObject;

    Image image;

    public Sprite disabledSprite, enabledSprite, selectedSprite;

    WeaponManaging managing;

    void Awake()
    {
        image = GetComponent<Image>();

        managing = FindAnyObjectByType<WeaponManaging>();
    }
    public void SetPlayerWeapon()
    {
        WeaponTransfer.startingWeapon = weaponObject;

        managing.ResetButtons(enabledSprite);

        image.sprite = selectedSprite;
    }
}