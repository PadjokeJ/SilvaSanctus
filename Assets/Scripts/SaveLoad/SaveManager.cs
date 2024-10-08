using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        PlayerPrefs.Save();
    }
    public static float RetrieveFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }

    public static void SaveInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }
    public static int RetrieveInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    public static void SaveBool(string name, bool value)
    {
        int intvalue = 0;

        if (value)
            intvalue = 1;
        PlayerPrefs.SetInt(name, intvalue);
        PlayerPrefs.Save();
    }
    public static bool RetrieveBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 1;
    }

    public static void SaveKey(string name, string type)
    {
        bool isDuplicate = false;

        string previousKeys;
        previousKeys = PlayerPrefs.GetString("keys");

        string[] keys = RetrieveAllKeys();
        foreach (string key in keys)
        {
            if (key.Split("-")[0] == name)
                isDuplicate = true;
        }
        if (!isDuplicate)
        {
            previousKeys += "_" + name + "-" + type;
            PlayerPrefs.SetString("keys", previousKeys);
        }
        PlayerPrefs.Save();
    }

    public static string[] RetrieveAllKeys()
    {
        string unCutKeys;
        unCutKeys = PlayerPrefs.GetString("keys");

        string[] keys = unCutKeys.Split("_");

        return keys;
    }

    public static string CompleteTutorial()
    {
        string filePath = Application.persistentDataPath;
        string fileName = ".tutorial";

        System.IO.File.WriteAllText(filePath + "/" + fileName, "");

        return filePath + "/" + fileName;
    }

    public static bool HasKey(string key)
    {
        bool has = PlayerPrefs.HasKey(key);
        return has;
    }

    public static void ClearPlayerData()
    {
        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("experience");
    }
}
