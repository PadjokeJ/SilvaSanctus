using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LanguageManager : MonoBehaviour
{
    bool active = false;
    public int langId;
    private void Awake()
    {
        if (Application.systemLanguage.ToString().StartsWith("French"))
            langId = 1;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langId];
    }
}
