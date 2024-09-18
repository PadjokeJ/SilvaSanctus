using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialWeapon;

    public static TutorialManager instance;

    Transition transition;

    private void Awake()
    {
        instance = this;
        PickUpWeapon();


        

        transition = FindAnyObjectByType<Transition>();
        transition.FadeToWhite();
    }

    public void PickUpWeapon()
    {
        WeaponTransfer.startingWeapon = tutorialWeapon;
    }

    void CompleteTutorial()
    {
        SaveManager.CompleteTutorial();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CompleteTutorial();
        StartCoroutine(ToLevel());
    }

    IEnumerator ToLevel()
    {
        transition.FadeToBlack();
        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene(2);
    }
}
