using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

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

    Transition transition;

    public AudioClip clickClip;

    public TextMeshProUGUI damage;
    public TextMeshProUGUI reloadTime;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI description;
    public Image weaponImage;

    public Sprite weaponSprite;

    private void Awake()
    {   
        int level = PlayerLevelling.GetLevel();

        mainMenu = FindAnyObjectByType<MainMenu>();

        WeaponTransfer.weaponsList = weapons;


        panelHidePos = new Vector2(800, 0);

        firstSquare = GenerateWeaponSelector(buttonPrefab, weapons.weaponPrefabs, 60, 240, panel.transform);
        firstSquare = transform.GetChild(0).GetChild(0).gameObject;

        panel.anchoredPosition = panelHidePos;

        hidden = true;

        transition = FindAnyObjectByType<Transition>();
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
        PlayClickAudio();
        hidden = true;

        mainMenu.WentBackToThis();
        mainMenu.currentPos = Vector2.zero;
    }

    GameObject GenerateWeaponSelector(GameObject prefab, GameObject[] arrayOfWeapons, int spacing, int maxWidth, Transform transform)
    {
        int x = spacing / 2, y = -spacing / 2;

        GameObject returner = null;

        int index = 0;

        List<GameObject> listOfWeapons = new List<GameObject>(arrayOfWeapons);
        int level = PlayerLevelling.GetLevel();
        Debug.Log((level));

        foreach (GameObject weapon in arrayOfWeapons)
        {
            if (x > maxWidth)
            {
                x = spacing / 2;
                y -= spacing;
            }

            

            GameObject obj = Instantiate<GameObject>(prefab, transform);

            WeaponSelector selector = obj.GetComponent<WeaponSelector>();

            selector.clickClip = clickClip;

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;
            selector.weaponObject = weapon;

            selector.weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;

            selector.damage = damage;
            selector.weaponName = weaponName;
            selector.description = description;
            selector.reloadTime = reloadTime;

            selector.weaponImage = weaponImage;
            List<int> listOfUnlockedWeapons = new List<int>(weapons.unlockedWeapons);

            if (listOfUnlockedWeapons[index] < level)
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
            hidden = true;
            StartCoroutine(PlayEnum());
            PlayClickAudio();
        }
    }

    IEnumerator PlayEnum()
    {
        transition.FadeToBlack();

        yield return new WaitForSeconds(0.6f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void ResetButtons(Sprite unselectedSprite)
    {

        foreach (Image image in listOfButtons)
        {
            image.sprite = unselectedSprite;
        }
    }

    void PlayClickAudio()
    {
        AudioManager.instance.PlayAudio(clickClip, Vector3.zero, 1f, 0.1f);
    }
}
