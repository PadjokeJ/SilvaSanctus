using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void FadeToWhite()
    {
        StartCoroutine(ColorLerp(new Color(0, 0, 0, 1f), new Color(0, 0, 0, 0f)));
        Debug.Log("Screen fades in");
    }
    public void FadeToBlack()
    {
        StartCoroutine(ColorLerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f)));
        Debug.Log("Screen fades out");
    }

    IEnumerator ColorLerp(Color original, Color target)
    {
        for (int i = 0; i <= 10; i++)
        {
            image.color = Color.Lerp(original, target, Mathf.Clamp(i/10f, 0f, 1f));
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
