using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Image textPanel;

    public static TutorialText instance;

    private void Awake()
    {
        instance = this;

        textPanel.color = new Color(0.2f, 0.2f, 0.2f, 0f);
        tmp.text = "";
    }
    IEnumerator WriteText(string text)
    {
        
        for (int i = 1; i < text.Length; i++)
        {
            tmp.text = text[0..i];
            yield return new WaitForSecondsRealtime(0.02f);

            if (Input.anyKeyDown)
                break;
        }
        tmp.text = text;

        yield return new WaitForSecondsRealtime(0.5f);

        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            if (Input.anyKey)
                break;
        }

        StartCoroutine(PanelHide());
        tmp.text = "";
        Time.timeScale = 1f;
    }

    IEnumerator PanelPopup(string text)
    {
        Color transparent = new Color(0.2f, 0.2f, 0.2f, 0f);
        Color filled = new Color(0.2f, 0.2f, 0.2f, 1f);

        for (int i = 0; i <= 20; i++)
        {
            textPanel.color = Color.Lerp(transparent, filled, i/20f);

            yield return new WaitForSecondsRealtime(0.02f);

            Time.timeScale = 1f - i / 20f;
        }

        StartCoroutine(WriteText(text));
    }

    IEnumerator PanelHide()
    {
        Color transparent = new Color(0.2f, 0.2f, 0.2f, 0f);
        Color filled = new Color(0.2f, 0.2f, 0.2f, 1f);

        for (int i = 0; i <= 20; i++)
        {
            textPanel.color = Color.Lerp(filled, transparent, i / 20f);

            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    public void PlayText(string text)
    {
        StartCoroutine(PanelPopup(text));
    }
}
