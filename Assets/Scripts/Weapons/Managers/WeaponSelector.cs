using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelector : Selectable
{
    public GameObject weaponObject;

    Image selectImage;

    public Sprite disabledSprite, enabledSprite, selectedSprite;

    public WeaponManaging managing;

    public AudioClip clickClip;

    bool isHighlighted = false;

    public Sprite weaponSprite;

    public TextMeshProUGUI damage;
    public TextMeshProUGUI reloadTime;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI description;
    public Image weaponImage;

    void Awake()
    {
        selectImage = GetComponent<Image>();

        managing = FindAnyObjectByType<WeaponManaging>();

        
    }

    private void Update()
    {
        if (IsHighlighted() && isHighlighted != IsHighlighted())
            OnHighlight();
        isHighlighted = IsHighlighted();
    }

    public void SetPlayerWeapon()
    {
        WeaponTransfer.startingWeapon = weaponObject;

        managing.ResetButtons(enabledSprite);

        selectImage.sprite = selectedSprite;

        PlayClickAudio();

    }

    public void OnHighlight()
    {
        GenericWeaponManager gwp = weaponObject.GetComponent<GenericWeaponManager>();

        damage.text = "Damage : " + gwp.weaponDamage.ToString();
        reloadTime.text = "Reload time : " + gwp.reloadTime.ToString();
        weaponName.text = gwp.weaponName;
        description.text = gwp.weaponDescription;

        weaponImage.sprite = weaponSprite;
    }

    void PlayClickAudio()
    {
        AudioManager.instance.PlayAudio(clickClip, Vector3.zero, 1f, 0.1f);
    }
}
