using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pop : MonoBehaviour
{
    string lastText;
    TextMeshProUGUI thisTMP;

    RectTransform rectTransform;
    private void Awake()
    {
        thisTMP = GetComponent<TextMeshProUGUI>();
        lastText = thisTMP.text;

        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (lastText != thisTMP.text)
            StartCoroutine(PopScale());
        lastText = thisTMP.text;
    }

    IEnumerator PopScale()
    {
        rectTransform.localScale = Vector3.one * 1.5f;
        for (int i = 0; i < 10; i++)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one, i / 10f);
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }

}
