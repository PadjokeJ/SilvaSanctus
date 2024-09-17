using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialWeapon;

    public static TutorialManager instance;

    private void Awake()
    {
        instance = this;
        PickUpWeapon();


        Transition transition;

        transition = FindAnyObjectByType<Transition>();
        transition.FadeToWhite();
    }

    public void PickUpWeapon()
    {
        WeaponTransfer.startingWeapon = tutorialWeapon;
    }
}
