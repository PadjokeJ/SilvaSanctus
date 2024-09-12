using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerLevelling
{
    public static int playerLevel;
    public static float experiencePoints;
    public static int levelAtStartOfRun;
    public static int GetLevel()
    {
        playerLevel = SaveManager.RetrieveInt("level");
        return playerLevel;
    }
    public static void InitDeltaLevel()
    {
        levelAtStartOfRun = playerLevel;
    }

    public static void GainExperience(float ammountGained)
    {
        int previousLevel = playerLevel;

        experiencePoints = SaveManager.RetrieveFloat("experience");
        experiencePoints += ammountGained;
        SaveManager.SaveFloat("experience", experiencePoints);
        Debug.Log(experiencePoints);

        playerLevel = UpdateLevel(experiencePoints);

        if (playerLevel > previousLevel) SaveManager.SaveInt("level", playerLevel);
    }

    static int UpdateLevel(float experience)
    {
        int level = 0;

        float levelCalc;
        levelCalc = Mathf.Pow(experience, 0.58f);
        levelCalc = levelCalc / 2.8f;

        level = Mathf.RoundToInt(levelCalc) + 1;

        return level;
    }

    static float MaxExp()
    {
        float reverseCalc;
        float level;
        level = SaveManager.RetrieveInt("level");
        //level -= 1;
        reverseCalc = level * 2.8f;
        reverseCalc = Mathf.Pow(reverseCalc, 1 / 0.58f);

        return reverseCalc;
    }
}
