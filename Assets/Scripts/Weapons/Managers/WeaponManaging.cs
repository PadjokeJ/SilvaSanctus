using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManaging : MonoBehaviour
{
    public Weapons weapons;
    public GameObject buttonPrefab;
    public RectTransform panel;

    public bool hidden;
    Vector2 panelHidePos, panelScreenPos;

    MainMenu mainMenu;

    private void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenu>();

        WeaponTransfer.weaponsList = weapons;

        WeaponTransfer.startingWeapon = weapons.weaponPrefabs[0];

        panelHidePos = new Vector2(800, 0);

        GenerateWeaponSelector(buttonPrefab, weapons.weaponPrefabs, 60, 240, panel.transform);


        panel.anchoredPosition = panelHidePos;

        hidden = true;


    }

    private void FixedUpdate()
    {
        if(hidden)
            panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, panelHidePos, 0.5f);
        else
            panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, panelScreenPos, 0.5f);
    }

    public void GoBack()
    {
        hidden = true;

        mainMenu.WentBackToThis();
        mainMenu.currentPos = Vector2.zero;
    }

    void GenerateWeaponSelector(GameObject prefab, GameObject[] listOfWeapons, int spacing, int maxWidth, Transform transform)
    {
        int x = 0, y = 0;

        foreach (GameObject weapon in listOfWeapons)
        {
            if (x > maxWidth)
            {
                x = 0;
                y += spacing;
            }
            GameObject obj = Instantiate<GameObject>(prefab, transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            obj.GetComponent<Image>().sprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;

            x += spacing;
        }
    }
}
