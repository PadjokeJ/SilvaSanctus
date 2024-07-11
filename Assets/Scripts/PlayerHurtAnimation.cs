using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHurtAnimation : MonoBehaviour
{
    Image hurtImage;
    Color transparent, redColor;
    void Awake()
    {
        hurtImage = GetComponent<Image>();

        redColor = Color.red;
        redColor.a = 0.2f;
        transparent = Color.red;
        transparent.a = 0f;
    }

    public void HurtScreen()
    {
        StartCoroutine(ColorLerpTo(redColor, transparent, 0.2f));
    }

    public IEnumerator ColorLerpTo(Color startColor, Color endColor, float _duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _duration)
        {
            hurtImage.color = Color.Lerp(startColor, endColor, (elapsedTime / _duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
