using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerLevelling
{
    public static int playerLevel;
    public static float expAtStartOfRun;
    public static float experiencePoints;
    public static int levelAtStartOfRun;
    public static int GetLevel()
    {
        experiencePoints = SaveManager.RetrieveFloat("experience");
        playerLevel = SaveManager.RetrieveInt("level");
        if (playerLevel == 0)
            playerLevel = 1;
        return playerLevel;
    }
    public static void InitDeltaLevel()
    {
        levelAtStartOfRun = SaveManager.RetrieveInt("level");
        expAtStartOfRun = SaveManager.RetrieveFloat("experience");

        Debug.Log($"Level at start of the run is {levelAtStartOfRun}");
        Debug.Log($"Exp at start of the run is {expAtStartOfRun}");
    }

    public static void GainExperience(float ammountGained)
    {
        int previousLevel = playerLevel;

        experiencePoints = SaveManager.RetrieveFloat("experience");
        experiencePoints += ammountGained;
        SaveManager.SaveFloat("experience", experiencePoints);
        

        playerLevel = UpdateLevel(experiencePoints);

        Debug.Log($"Player has {experiencePoints} EXP : -> {playerLevel}");

        if (playerLevel > previousLevel) SaveManager.SaveInt("level", playerLevel);
    }

    public static int UpdateLevel(float experience)
    {
        int level = 0;

        float levelCalc;
        levelCalc = Mathf.Pow(experience, 0.58f);
        levelCalc = levelCalc / 2.8f;

        level = Mathf.RoundToInt(levelCalc) + 1;

        return level;
    }

    public static float MaxExp(int level)
    {
        float reverseCalc;
        //level -= 1;
        reverseCalc = level * 2.8f;
        reverseCalc = Mathf.Pow(reverseCalc, 1 / 0.58f);

        return reverseCalc;
    }
}
