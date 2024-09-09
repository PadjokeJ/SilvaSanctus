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

    public GameObject firstSquare;

    public Sprite disabledSprite, enabledSprite, selectedSprite;

    List<Image> listOfButtons = new List<Image>();

    private void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenu>();

        WeaponTransfer.weaponsList = weapons;


        panelHidePos = new Vector2(800, 0);

        firstSquare = GenerateWeaponSelector(buttonPrefab, weapons.weaponPrefabs, 60, 240, panel.transform);


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

    GameObject GenerateWeaponSelector(GameObject prefab, GameObject[] listOfWeapons, int spacing, int maxWidth, Transform transform)
    {
        int x = spacing / 2, y = -spacing / 2;

        GameObject returner = null;

        int index = 0;

        List<int> listOfUnlockedWeapons = new List<int>(weapons.unlockedWeapons);

        foreach (GameObject weapon in listOfWeapons)
        {
            if (x > maxWidth)
            {
                x = spacing / 2;
                y -= spacing;
            }

            

            GameObject obj = Instantiate<GameObject>(prefab, transform);

            WeaponSelector selector = obj.GetComponent<WeaponSelector>();

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;
            selector.weaponObject = weapon;



            if (listOfUnlockedWeapons.Contains(index))
            {
                obj.GetComponent<Image>().sprite = selector.enabledSprite;
                listOfButtons.Add(obj.GetComponent<Image>());
            }
            else
            {
                obj.GetComponent<Image>().sprite = selector.disabledSprite;
                obj.GetComponent<Button>().interactable = false;
            }

            if (returner == null)
                returner = obj;

            x += spacing;
            index++;
        }

        return returner;
    }

    public void Play()
    {
        if (WeaponTransfer.startingWeapon != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            hidden = true;
        }
    }

    public void ResetButtons(Sprite unselectedSprite)
    {

        foreach (Image image in listOfButtons)
        {
            image.sprite = unselectedSprite;
        }
    }
}