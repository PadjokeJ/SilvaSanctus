using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTester : MonoBehaviour
{
    public static BossTester instance;
    public GameObject testingWeapon;

    Transition transition;
    private void Awake()
    {
        instance = this;
        
        WeaponTransfer.startingWeapon = testingWeapon;
        
        transition = FindAnyObjectByType<Transition>();
        transition.FadeToWhite();
    }
}
