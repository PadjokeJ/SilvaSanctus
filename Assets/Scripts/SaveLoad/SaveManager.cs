using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
    public float RetrieveFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }

    public void SaveInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    public int RetrieveInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }
}
