using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelling : MonoBehaviour
{
    public int playerLevel;
    public float experiencePoints;
    void Awake()
    {
        //dont destroy on load
        PlayerLevelling[] objs = FindObjectsOfType<PlayerLevelling>();
        if (objs.Length > 1) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);


        playerLevel = SaveManager.RetrieveInt("level");
    }

    public void gainExperience(float ammountGained)
    {
        experiencePoints += ammountGained;

        int previousLevel = playerLevel;
        playerLevel = UpdateLevel(experiencePoints);

        if (playerLevel > previousLevel) SaveManager.SaveInt("level", playerLevel);
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
