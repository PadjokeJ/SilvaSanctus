using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelling : MonoBehaviour
{
    public int playerLevel;
    public float experiencePoints;

    SaveManager saveManager;
    void Awake()
    {
        //dont destroy on load
        PlayerLevelling[] objs = FindObjectsOfType<PlayerLevelling>();
        if (objs.Length > 1) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        saveManager = GetComponent<SaveManager>();

        playerLevel = saveManager.RetrieveInt("level");
    }

    public void gainExperience(float ammountGained)
    {
        experiencePoints += ammountGained;

        int previousLevel = playerLevel;
        playerLevel = UpdateLevel(experiencePoints);

        if (playerLevel > previousLevel) saveManager.SaveInt("level", playerLevel);
    }

    int UpdateLevel(float experience)
    {
        int level = 0;

        float levelCalc;
        levelCalc = Mathf.Pow(experience, 0.58f);
        levelCalc = levelCalc / 2.8f;

        level = Mathf.RoundToInt(levelCalc) + 1;

        return level;
    }
}
