using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    public bool isEN;

    void EnglishForm()
    {
        Application.OpenURL("https://forms.gle/yLwj9xbHuwKD77Lm7");
    }
    void FrenchForm()
    {
        Application.OpenURL("https://forms.gle/yLwj9xbHuwKD77Lm7");
    }

    public void SendFeedback()
    {
        if (isEN)
            EnglishForm();
        else
            FrenchForm();
    }
}
