using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Feedback : MonoBehaviour
{
    public bool isEN;

    void EnglishForm()
    {
        Application.OpenURL("https://forms.gle/yLwj9xbHuwKD77Lm7");
    }
    void FrenchForm()
    {
        Application.OpenURL("https://forms.gle/6mJHgQfyqBtEMpgHA");
    }

    public void SendFeedback()
    {
        if (LocalizationSettings.SelectedLocale.name.StartsWith("En"))
            EnglishForm();
        else
            FrenchForm();
    }
}
