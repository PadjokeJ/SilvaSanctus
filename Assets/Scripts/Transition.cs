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
        //image.color = Color.black;
    }

    public void FadeToWhite()
    {
        StartCoroutine(ColorLerp(new Color(0, 0, 0, 1f), new Color(0, 0, 0, 0f)));
        Debug.Log("white screen");
    }
    public void FadeToBlack()
    {
        StartCoroutine(ColorLerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f)));
        Debug.Log("black screen");
    }

    IEnumerator ColorLerp(Color original, Color target)
    {
        for (int i = 0; i <= 20; i++)
        {
            image.color = Color.Lerp(original, target, Mathf.Clamp(i/20f, 0f, 1f));
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
