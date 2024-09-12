using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerLevelling
{
    public static int playerLevel;
    public static float experiencePoints;
    public static int GetLevel()
    {
        playerLevel = SaveManager.RetrieveInt("level");
        return playerLevel;
    }

    public static void GainExperience(float ammountGained)
    {
        experiencePoints += ammountGained;

        int previousLevel = playerLevel;
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
}
