using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetaTools : MonoBehaviour
{
    public GameObject tools;

    private void Awake()
    {
        tools.SetActive(false);
    }
    
    public void ActivateBetaTools()
    {
        tools.SetActive(!tools.activeSelf);
    }
    public void DeleteData()
    {
        SaveManager.ClearPlayerData();
        int i = PlayerLevelling.UpdateLevel(0f);
        SceneManager.LoadScene(0);
    }

    public void ReplayTutorial()
    {
        SceneManager.LoadScene(1);
    }
}
